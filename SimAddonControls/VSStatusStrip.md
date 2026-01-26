# VSStatusStrip - StatusStrip au style Visual Studio

Un contrôle StatusStrip personnalisé pour Windows Forms (.NET 8) avec un style moderne inspiré de Visual Studio.

## Caractéristiques

? **Entièrement personnalisable** - Toutes les couleurs sont configurables
? **Thèmes prédéfinis** - Dark, Blue, Light
? **Renderer personnalisé** - Dessin optimisé avec double-buffering
? **Bordure supérieure** - Ligne de séparation subtile
? **Grip personnalisé** - Poignée de redimensionnement stylisée
? **Designer-friendly** - Utilisable directement dans le Designer Visual Studio

## Utilisation

### 1. Dans le code

```csharp
using SimAddonControls;

// Créer le contrôle
var statusStrip = new VSStatusStrip();

// Appliquer un thème prédéfini
statusStrip.ApplyVisualStudioDarkTheme();

// Ou personnaliser les couleurs individuellement
statusStrip.BackColor = Color.FromArgb(0, 122, 204);
statusStrip.ForeColor = Color.White;
statusStrip.BorderColor = Color.FromArgb(0, 100, 180);
statusStrip.GripColor = Color.FromArgb(70, 70, 74);

// Ajouter des éléments
statusStrip.Items.Add(new ToolStripStatusLabel("Ready"));
statusStrip.Items.Add(new ToolStripStatusLabel("Connected"));
```

### 2. Dans le Designer

1. Compilez le projet `SimAddonControls`
2. Le contrôle `VSStatusStrip` apparaîtra dans la Toolbox
3. Glissez-déposez le contrôle sur votre formulaire
4. Configurez les propriétés dans la fenêtre Propriétés

### 3. Remplacer un StatusStrip existant

Dans `Form1.Designer.cs`, remplacez :
```csharp
private System.Windows.Forms.StatusStrip statusStrip;
```

Par :
```csharp
private SimAddonControls.VSStatusStrip statusStrip;
```

Et dans `InitializeComponent()` :
```csharp
this.statusStrip = new SimAddonControls.VSStatusStrip();
```

## Propriétés personnalisables

| Propriété | Description | Valeur par défaut (Dark) |
|-----------|-------------|--------------------------|
| `BackColor` | Couleur de fond | RGB(0, 122, 204) - Bleu VS |
| `ForeColor` | Couleur du texte | White |
| `BorderColor` | Couleur de la bordure supérieure | RGB(0, 100, 180) |
| `GripColor` | Couleur de la poignée | RGB(70, 70, 74) |

## Thèmes prédéfinis

### Visual Studio Dark (par défaut)
```csharp
statusStrip.ApplyVisualStudioDarkTheme();
```
- Fond: Bleu VS (0, 122, 204)
- Texte: Blanc
- Bordure: Bleu foncé (0, 100, 180)
- Grip: Gris foncé (70, 70, 74)

### Visual Studio Blue
```csharp
statusStrip.ApplyVisualStudioBlueTheme();
```

### Visual Studio Light
```csharp
statusStrip.ApplyVisualStudioLightTheme();
```

### Thème personnalisé
```csharp
statusStrip.ApplyCustomDarkTheme(Color.DimGray, Color.White);
```

## Exemple complet dans Form1

### Avant (StatusStrip standard)
```csharp
// Form1.Designer.cs
private System.Windows.Forms.StatusStrip statusStrip;

// InitializeComponent()
this.statusStrip = new StatusStrip();
this.statusStrip.BackColor = Color.Gray; // Pas vraiment personnalisable
```

### Après (VSStatusStrip)
```csharp
// Form1.Designer.cs
private SimAddonControls.VSStatusStrip statusStrip;

// InitializeComponent()
this.statusStrip = new SimAddonControls.VSStatusStrip();
// Pas de configuration nécessaire - thème par défaut appliqué

// OU dans Form1_Load()
statusStrip.ApplyVisualStudioDarkTheme();
```

## Architecture

Le contrôle est composé de deux fichiers :

- **VSStatusStrip.cs** - Implémentation principale avec renderer personnalisé
- **VSStatusStrip.Designer.cs** - Méthodes de thèmes et helpers

### Renderer Personnalisé

Le `VSStatusStripRenderer` gère :
- ? Dessin du fond avec la couleur personnalisée
- ? Bordure supérieure pour séparer du contenu
- ? Grip de redimensionnement stylisé (3x3 points)
- ? Séparateurs avec couleurs personnalisées
- ? Texte avec couleur de premier plan personnalisée

## Rendu Visuel

```
???????????????????????????????????????????????????
?                                                 ?
?           Contenu du formulaire                 ?
?                                                 ?
??????????????????????????????????????????????????? ? Bordure supérieure
? Status: Ready ? Connected ? 12:34:56 Z    ???? ? VSStatusStrip
???????????????????????????????????????????????????
```

## Intégration dans Form1

### Étape 1: Modifier Form1.Designer.cs

```csharp
using SimAddonControls;

namespace SimAddon
{
    partial class Form1
    {
        // Changer la déclaration
        private VSStatusStrip statusStrip;
        
        private void InitializeComponent()
        {
            // ...
            statusStrip = new VSStatusStrip();
            lblConnectionStatus = new ToolStripStatusLabel();
            lblPluginStatus = new ToolStripStatusLabel();
            toolStripHeureZulu = new ToolStripStatusLabel();
            
            // Configuration du StatusStrip
            statusStrip.Items.AddRange(new ToolStripItem[] { 
                lblConnectionStatus, 
                lblPluginStatus, 
                toolStripHeureZulu 
            });
            statusStrip.Location = new Point(0, 846);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(590, 24);
            statusStrip.TabIndex = 6;
            // Pas besoin de définir BackColor/ForeColor - thème par défaut
            
            // Ajouter au formulaire
            this.Controls.Add(statusStrip);
        }
    }
}
```

### Étape 2: Modifier Form1.cs (optionnel)

```csharp
private async void Form1_Load(object sender, EventArgs e)
{
    // Appliquer le thème si vous voulez changer du défaut
    statusStrip.ApplyVisualStudioDarkTheme();
    
    // OU thème personnalisé pour correspondre au reste de l'interface
    statusStrip.BackColor = Color.FromArgb(0, 122, 204);
    
    // Le reste du code...
}
```

## Compatibilité avec les ToolStripItems existants

? **ToolStripStatusLabel** - Fonctionne parfaitement
? **ToolStripProgressBar** - Supporté
? **ToolStripDropDownButton** - Supporté
? **ToolStripSplitButton** - Supporté
? **Séparateurs** - Dessin personnalisé appliqué

## Avantages par rapport au StatusStrip standard

| Feature | StatusStrip Standard | VSStatusStrip |
|---------|---------------------|---------------|
| Couleur de fond | ? Limitée | ? Complète |
| Bordure personnalisée | ? Non | ? Oui |
| Grip stylisé | ? Basique | ? Personnalisé |
| Thèmes prêts | ? Non | ? 3 thèmes |
| Renderer optimisé | ?? Standard | ? Double-buffer |

## Notes Techniques

- Utilise `ToolStripProfessionalRenderer` comme base
- Override des méthodes `OnRender*` pour personnaliser l'apparence
- `SetStyle` avec `OptimizedDoubleBuffer` pour un rendu fluide
- Couleurs des items mises à jour automatiquement lors de l'ajout

## Exemples de Couleurs

### Correspondre au thème sombre de Form1
```csharp
statusStrip.BackColor = Color.FromArgb(0, 122, 204);  // Bleu VS
statusStrip.ForeColor = Color.White;
statusStrip.BorderColor = Color.FromArgb(0, 100, 180);
```

### Barre de statut neutre grise
```csharp
statusStrip.BackColor = Color.DimGray;
statusStrip.ForeColor = Color.White;
statusStrip.BorderColor = Color.FromArgb(100, 100, 100);
```

### Style "Success" (vert)
```csharp
statusStrip.BackColor = Color.FromArgb(76, 175, 80);  // Material Green
statusStrip.ForeColor = Color.White;
statusStrip.BorderColor = Color.FromArgb(56, 142, 60);
```

## Compatibilité

- ? .NET 8 Windows Forms
- ? Visual Studio 2022
- ? Windows 10/11
- ? Compatible avec les contrôles VSTabControl et LedBulb

## Migration Rapide

Pour migrer rapidement un StatusStrip existant :

1. Remplacer les déclarations :
   ```csharp
   // Avant
   private StatusStrip statusStrip;
   // Après
   private VSStatusStrip statusStrip;
   ```

2. Remplacer l'instanciation :
   ```csharp
   // Avant
   this.statusStrip = new StatusStrip();
   // Après
   this.statusStrip = new VSStatusStrip();
   ```

3. Supprimer les configurations de couleurs manuelles (optionnel)

4. (Optionnel) Appliquer un thème dans Form_Load :
   ```csharp
   statusStrip.ApplyVisualStudioDarkTheme();
   ```

C'est tout ! ??
