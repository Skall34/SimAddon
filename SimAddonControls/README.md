# SimAddonControls - Bibliothèque de Contrôles Personnalisés

Une collection de contrôles Windows Forms personnalisés avec un style moderne inspiré de Visual Studio.

## ?? Contrôles Disponibles

### 1. ?? **VSTabControl** - TabControl Personnalisé
Style moderne avec des onglets colorés et des effets de survol.

**Fichiers:**
- `VSTabControl.cs`
- `VSTabControl.Designer.cs`
- `VSTabControl.md`

**Fonctionnalités:**
- ? 7 couleurs personnalisables
- ? 3 thèmes prédéfinis (Dark, Blue, Light)
- ? Ligne d'accent sur l'onglet sélectionné
- ? Effets de survol (hover)
- ? Double-buffering pour un rendu fluide

**Usage:**
```csharp
var tabControl = new VSTabControl();
tabControl.ApplyVisualStudioDarkTheme();
```

---

### 2. ?? **VSStatusStrip** - StatusStrip Personnalisé
Barre d'état stylisée avec couleurs personnalisables.

**Fichiers:**
- `VSStatusStrip.cs`
- `VSStatusStrip.Designer.cs`
- `VSStatusStrip.md`

**Fonctionnalités:**
- ? Couleurs de fond et de texte personnalisables
- ? Bordure supérieure pour séparation
- ? Grip de redimensionnement stylisé
- ? Thèmes prédéfinis
- ? Renderer personnalisé optimisé

**Usage:**
```csharp
var statusStrip = new VSStatusStrip();
statusStrip.ApplyVisualStudioDarkTheme();
```

---

### 3. ?? **LedBulb** - Indicateur LED
Contrôle LED pour afficher des états visuels (connecté/déconnecté, etc.).

**Fichiers:**
- `LedBulb.cs`

**Fonctionnalités:**
- ? LED allumée/éteinte
- ? Couleur personnalisable
- ? Effet de brillance réaliste
- ? Clignotement possible
- ? Rendu avec gradients

**Usage:**
```csharp
var led = new LedBulb();
led.On = true;
led.Color = Color.Lime;
```

---

## ?? Thèmes Cohérents

Tous les contrôles supportent les mêmes thèmes pour une interface cohérente :

### Visual Studio Dark (Défaut)
```csharp
tabControl1.ApplyVisualStudioDarkTheme();
statusStrip.ApplyVisualStudioDarkTheme();
```

### Visual Studio Blue
```csharp
tabControl1.ApplyVisualStudioBlueTheme();
statusStrip.ApplyVisualStudioBlueTheme();
```

### Visual Studio Light
```csharp
tabControl1.ApplyVisualStudioLightTheme();
statusStrip.ApplyVisualStudioLightTheme();
```

---

## ?? Utilisation dans SimAddon

### Form1.Designer.cs
```csharp
using SimAddonControls;

namespace SimAddon
{
    partial class Form1
    {
        private VSTabControl tabControl1;
        private VSStatusStrip statusStrip;
        
        private void InitializeComponent()
        {
            // TabControl
            this.tabControl1 = new VSTabControl();
            this.tabControl1.Dock = DockStyle.Fill;
            
            // StatusStrip
            this.statusStrip = new VSStatusStrip();
            this.statusStrip.Dock = DockStyle.Bottom;
            
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip);
        }
    }
}
```

### Form1.cs - Form_Load
```csharp
private async void Form1_Load(object sender, EventArgs e)
{
    // Appliquer les thèmes
    tabControl1.ApplyVisualStudioDarkTheme();
    statusStrip.ApplyVisualStudioDarkTheme();
    
    // Le reste de votre code...
}
```

---

## ?? Architecture du Projet

```
SimAddonControls/
??? VSTabControl.cs              # TabControl personnalisé
??? VSTabControl.Designer.cs     # Thèmes TabControl
??? VSTabControl.md             # Documentation TabControl
??? VSStatusStrip.cs            # StatusStrip personnalisé
??? VSStatusStrip.Designer.cs   # Thèmes StatusStrip
??? VSStatusStrip.md           # Documentation StatusStrip
??? LedBulb.cs                 # Contrôle LED
??? SimAddonControls.csproj    # Fichier projet
```

---

## ?? Palette de Couleurs Visual Studio

### Thème Dark

| Élément | Couleur | RGB |
|---------|---------|-----|
| **TabControl** |
| Fond onglet | Gris foncé | (45, 45, 48) |
| Onglet sélectionné | Bleu nuit | MidnightBlue |
| Onglet survol | Gris moyen | DarkGray |
| Ligne d'accent | Bleu VS | (0, 122, 204) |
| Fond contenu | Gris très foncé | (37, 37, 38) |
| **StatusStrip** |
| Fond | Bleu VS | (0, 122, 204) |
| Texte | Blanc | White |
| Bordure | Bleu foncé | (0, 100, 180) |
| Grip | Gris foncé | (70, 70, 74) |
| **LED** |
| Connecté | Vert | Lime |
| Déconnecté | Rouge | Red |

---

## ?? Migration depuis les Contrôles Standard

### Avant (Contrôles Standard)
```csharp
// Déclarations
private TabControl tabControl1;
private StatusStrip statusStrip;

// Initialisation
this.tabControl1 = new TabControl();
this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
this.tabControl1.DrawItem += TabControl1_DrawItem;

this.statusStrip = new StatusStrip();
this.statusStrip.BackColor = Color.Gray;
```

### Après (Contrôles Personnalisés)
```csharp
// Déclarations
private VSTabControl tabControl1;
private VSStatusStrip statusStrip;

// Initialisation
this.tabControl1 = new VSTabControl();
// Pas de DrawMode ni DrawItem nécessaire !

this.statusStrip = new VSStatusStrip();
// Couleurs gérées par le thème

// Dans Form_Load
tabControl1.ApplyVisualStudioDarkTheme();
statusStrip.ApplyVisualStudioDarkTheme();
```

**Résultat:** ~100 lignes de code en moins ! ?

---

## ?? Avantages

### ? **Code Simplifié**
- Plus de méthodes DrawItem complexes
- Thèmes en une ligne de code
- Gestion automatique des couleurs

### ? **Réutilisable**
- Même contrôles dans tous vos projets
- Bibliothèque autonome
- Facilement extensible

### ? **Maintenable**
- Code centralisé
- Modifications dans un seul endroit
- Documentation complète

### ? **Performant**
- Double-buffering activé
- Rendu optimisé
- Pas de scintillement

### ? **Designer-Friendly**
- Apparaît dans la Toolbox VS
- Configuration visuelle
- IntelliSense complet

---

## ?? Documentation Détaillée

Chaque contrôle a sa propre documentation détaillée :

- **[VSTabControl.md](VSTabControl.md)** - Guide complet du TabControl
- **[VSStatusStrip.md](VSStatusStrip.md)** - Guide complet du StatusStrip

---

## ?? Configuration Requise

- ? .NET 8 Windows Forms
- ? Visual Studio 2022 (recommandé)
- ? Windows 10/11

---

## ?? Personnalisation Avancée

### Créer un Thème Personnalisé

```csharp
public class MyCustomTheme
{
    public static void ApplyToTabControl(VSTabControl tab)
    {
        tab.TabBackColor = Color.FromArgb(30, 30, 30);
        tab.TabSelectedBackColor = Color.FromArgb(0, 150, 136);
        tab.AccentColor = Color.Teal;
        tab.TabPageBackColor = Color.FromArgb(25, 25, 25);
    }
    
    public static void ApplyToStatusStrip(VSStatusStrip status)
    {
        status.BackColor = Color.FromArgb(0, 150, 136);
        status.ForeColor = Color.White;
        status.BorderColor = Color.FromArgb(0, 121, 107);
    }
}

// Utilisation
MyCustomTheme.ApplyToTabControl(tabControl1);
MyCustomTheme.ApplyToStatusStrip(statusStrip);
```

---

## ?? Debugging

### TabControl n'affiche pas les couleurs
? Vérifiez que `ApplyVisualStudioDarkTheme()` est appelé **après** `InitializeComponent()`

### StatusStrip garde sa couleur par défaut
? Assurez-vous d'utiliser `VSStatusStrip` et pas `StatusStrip`

### LED ne s'affiche pas
? Vérifiez que `LedBulb.Size` est définie (minimum 12x12)

---

## ?? Roadmap / Améliorations Futures

- [ ] **VSMenuStrip** - MenuStrip personnalisé
- [ ] **VSToolStrip** - ToolStrip personnalisé
- [ ] **VSButton** - Bouton avec style VS
- [ ] **VSTextBox** - TextBox avec bordure stylisée
- [ ] **VSComboBox** - ComboBox personnalisé
- [ ] **Plus de thèmes** - Dark+, Monokai, Dracula, etc.

---

## ?? Exemples

Voir le projet **SimAddon** pour des exemples d'utilisation réelle de tous les contrôles.

---

## ?? Contribution

Pour ajouter un nouveau contrôle personnalisé :

1. Créer `VSNomControle.cs` avec l'implémentation
2. Créer `VSNomControle.Designer.cs` avec les thèmes
3. Créer `VSNomControle.md` avec la documentation
4. Ajouter au README principal
5. Tester avec les thèmes existants

---

## ?? Licence

Même licence que le projet SimAddon principal.

---

## ?? Crédits

Développé pour le projet **SimAddon** par JFK & PB.

Inspiré par le look & feel de **Visual Studio 2022**.
