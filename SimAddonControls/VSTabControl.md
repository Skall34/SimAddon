# VSTabControl - TabControl au style Visual Studio

Un contrôle TabControl personnalisé pour Windows Forms (.NET 8) avec un style moderne inspiré de Visual Studio.

## Caractéristiques

? **Entièrement personnalisable** - Toutes les couleurs sont configurables
? **Thèmes prédéfinis** - Dark, Blue, Light
? **Effets visuels** - Hover, sélection avec ligne d'accent
? **Double-buffering** - Rendu fluide sans scintillement
? **Designer-friendly** - Utilisable directement dans le Designer Visual Studio

## Utilisation

### 1. Dans le code

```csharp
using SimAddonControls;

// Créer le contrôle
var tabControl = new VSTabControl();

// Appliquer un thème prédéfini
tabControl.ApplyVisualStudioDarkTheme();

// Ou personnaliser les couleurs individuellement
tabControl.TabBackColor = Color.FromArgb(45, 45, 48);
tabControl.TabSelectedBackColor = Color.MidnightBlue;
tabControl.AccentColor = Color.FromArgb(0, 122, 204);
```

### 2. Dans le Designer

1. Compilez le projet `SimAddonControls`
2. Le contrôle `VSTabControl` apparaîtra dans la Toolbox
3. Glissez-déposez le contrôle sur votre formulaire
4. Configurez les propriétés dans la fenêtre Propriétés

### 3. Remplacer un TabControl existant

Dans `Form1.Designer.cs`, remplacez :
```csharp
private System.Windows.Forms.TabControl tabControl1;
```

Par :
```csharp
private SimAddonControls.VSTabControl tabControl1;
```

Et dans `InitializeComponent()` :
```csharp
this.tabControl1 = new SimAddonControls.VSTabControl();
```

## Propriétés personnalisables

| Propriété | Description | Valeur par défaut (Dark) |
|-----------|-------------|--------------------------|
| `TabBackColor` | Couleur de fond des onglets | RGB(45, 45, 48) |
| `TabSelectedBackColor` | Couleur de l'onglet sélectionné | MidnightBlue |
| `TabHoverBackColor` | Couleur au survol | DarkGray |
| `TabTextColor` | Couleur du texte | White |
| `TabSelectedTextColor` | Couleur du texte sélectionné | White |
| `AccentColor` | Couleur de la ligne d'accent | RGB(0, 122, 204) |
| `TabPageBackColor` | Couleur du contenu des pages | RGB(37, 37, 38) |

## Thèmes prédéfinis

### Visual Studio Dark (par défaut)
```csharp
tabControl.ApplyVisualStudioDarkTheme();
```

### Visual Studio Blue
```csharp
tabControl.ApplyVisualStudioBlueTheme();
```

### Visual Studio Light
```csharp
tabControl.ApplyVisualStudioLightTheme();
```

## Exemple complet

```csharp
// Dans Form1.cs ou Form1_Load
var tabControl = new VSTabControl
{
    Dock = DockStyle.Fill,
    Location = new Point(0, 24),
    Size = new Size(800, 600)
};

// Appliquer le thème dark
tabControl.ApplyVisualStudioDarkTheme();

// Ajouter des pages
var page1 = new TabPage("Dashboard");
var page2 = new TabPage("Settings");
tabControl.TabPages.Add(page1);
tabControl.TabPages.Add(page2);

// Ajouter au formulaire
this.Controls.Add(tabControl);
```

## Architecture

Le contrôle est composé de deux fichiers :

- **VSTabControl.cs** - Implémentation principale du contrôle
- **VSTabControl.Designer.cs** - Méthodes de thèmes et helpers

## Rendu

```
????????????????????????????????????????????
? [Tab1] [Tab2] [Tab3] ????????????????? ? ? Barre d'onglets
? ?????                                    ? ? Ligne d'accent (onglet sélectionné)
????????????????????????????????????????????
?                                          ?
?   Contenu de la page                     ?
?                                          ?
????????????????????????????????????????????
```

## Compatibilité

- ? .NET 8 Windows Forms
- ? Visual Studio 2022
- ? Windows 10/11

## Notes de développement

Le contrôle utilise :
- `OnPaint` pour dessiner l'arrière-plan complet
- `OnDrawItem` pour dessiner chaque onglet individuellement
- `UserPaint` + `AllPaintingInWmPaint` + `OptimizedDoubleBuffer` pour un rendu optimal
- `OnControlAdded` pour appliquer automatiquement `TabPageBackColor` aux nouvelles pages
