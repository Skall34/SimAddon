# LED de Connexion Serveur - Documentation

## ???? Indicateur Visuel d'État de Connexion

Une LED a été ajoutée à la barre de menu de SimAddon pour indiquer visuellement l'état de la connexion au serveur web.

## ?? Emplacement

La LED est située dans la barre de menu (MenuStrip), **juste à gauche du bouton Minimize** (_), à droite dans la barre.

```
???????????????????????????????????????????????????????
? File  Network  Links  Help    [?] [X] [?] [_]     ?
?                                 ?                   ?
?                            LED ici                  ?
???????????????????????????????????????????????????????
```

## ?? États de la LED

| État | Couleur | Description |
|------|---------|-------------|
| ?? **Déconnecté** | Rouge | L'utilisateur n'est pas connecté au serveur web |
| ?? **Connecté** | Vert (Lime) | L'utilisateur est authentifié et connecté au serveur |

## ?? Mise à Jour de l'État

La LED est automatiquement mise à jour dans les situations suivantes :

### 1. **Au démarrage de l'application**
```csharp
// Dans le constructeur Form1()
UpdateServerConnectionLed(false); // Initialisé à déconnecté
```

### 2. **Lors de la connexion**
```csharp
// Dans connectToSite()
if (loginSuccess)
{
    UpdateServerConnectionLed(true); // LED verte
}
else
{
    UpdateServerConnectionLed(false); // LED rouge
}
```

### 3. **Lors de la déconnexion**
```csharp
// Menu: File > Logout
private void logoutToolStripMenuItem1_Click(...)
{
    _simData.logoutFromSite();
    UpdateServerConnectionLed(false); // LED rouge
}
```

### 4. **Lors de la vérification de session**
```csharp
// Menu: File > Check Session
private async void checkSessionToolStripMenuItem1_Click(...)
{
    bool sessionValid = await _simData.checkSession(...);
    UpdateServerConnectionLed(sessionValid); // LED selon l'état
}
```

## ?? Implémentation Technique

### Contrôle Utilisé
Le contrôle `LedBulb` du projet `SimAddonControls` est encapsulé dans un `ToolStripControlHost` pour pouvoir l'ajouter au MenuStrip.

### Code d'Intégration

**Form1.Designer.cs** :
```csharp
// Déclaration
private ToolStripControlHost ledConnectionStatus;

// Initialisation
ledConnectionStatus = new ToolStripControlHost(new SimAddonControls.LedBulb());
ledConnectionStatus.Alignment = ToolStripItemAlignment.Right;
ledConnectionStatus.ToolTipText = "État de connexion au serveur";
((SimAddonControls.LedBulb)ledConnectionStatus.Control).Size = new Size(16, 16);
((SimAddonControls.LedBulb)ledConnectionStatus.Control).On = false;
((SimAddonControls.LedBulb)ledConnectionStatus.Control).Color = Color.Red;

// Ajout au MenuStrip (avant btnMinimize)
menuStrip1.Items.AddRange(new ToolStripItem[] { 
    ..., helpToolStripMenuItem, btnClose, btnMaximize, ledConnectionStatus, btnMinimize 
});
```

**Form1.cs** :
```csharp
/// <summary>
/// Met à jour l'état de la LED de connexion au serveur
/// </summary>
private void UpdateServerConnectionLed(bool isConnected)
{
    if (ledConnectionStatus?.Control is SimAddonControls.LedBulb led)
    {
        led.On = isConnected;
        led.Color = isConnected ? Color.Lime : Color.Red;
        ledConnectionStatus.ToolTipText = isConnected 
            ? "Connecté au serveur" 
            : "Déconnecté du serveur";
    }
}
```

## ??? Interaction Utilisateur

- **Tooltip** : Survoler la LED affiche l'état de connexion en texte
  - "Connecté au serveur" (vert)
  - "Déconnecté du serveur" (rouge)

- **Aucun clic** : La LED est purement informative, elle ne réagit pas aux clics

## ?? Diagramme de Flux

```
???????????????????
? Démarrage App   ?
???????????????????
         ? LED = ??
         ?
???????????????????     Succès    ???????????????????
? Tentative Login ? ????????????  ? LED = ??        ?
???????????????????               ???????????????????
         ?                                 ?
         ? Échec                           ?
         ?                                 ?
???????????????????               ???????????????????
? LED = ??        ? ????????????? ? Logout          ?
???????????????????               ???????????????????
```

## ?? Avantages

? **Visibilité immédiate** - L'utilisateur voit d'un coup d'œil s'il est connecté
? **Non-intrusif** - Petit indicateur discret dans la barre de menu
? **Cohérent** - Utilise le contrôle LedBulb déjà existant
? **Mis à jour automatiquement** - Synchronisé avec l'état de session réel

## ?? Améliorations Futures Possibles

- ?? Ajouter un état "En cours de connexion" (LED jaune clignotante)
- ?? Permettre un clic sur la LED pour ouvrir une fenêtre de statut détaillé
- ?? Afficher le temps depuis la dernière connexion dans le tooltip
- ?? Vérification périodique automatique de la session

## ?? Notes

- La LED utilise le contrôle existant `LedBulb` de `SimAddonControls`
- L'API du LedBulb utilise `On` (bool) et `Color` (Color)
- La LED a une taille de 16x16 pixels
- Position: `Alignment = ToolStripItemAlignment.Right`
