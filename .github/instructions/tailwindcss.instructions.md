---
description: 'Tailwind CSS styling patterns for the dark theme UI'
applyTo: '**/*.razor,**/*.html,**/*.css'
---

# Tailwind CSS Instructions

## Dark Theme Patterns

The application uses a dark theme throughout with the slate color palette.

### Color Palette

- Background: `bg-slate-900` (body), `bg-slate-800` (cards)
- Text: `text-slate-100` (headings), `text-slate-300` (body), `text-slate-400` (muted)
- Borders: `border-slate-700`
- Accents: `text-blue-400`, `bg-blue-600` (buttons), `text-purple-300`
- Header: `bg-blue-700`

### Card Pattern

```html
<div class="bg-slate-800/60 backdrop-blur-sm rounded-xl overflow-hidden shadow-lg border border-slate-700/50 hover:border-blue-500/50 hover:shadow-blue-500/10 hover:shadow-xl transition-all duration-300 hover:translate-y-[-6px]">
    <div class="p-6">
        <h3 class="text-xl font-semibold text-slate-100">Title</h3>
        <p class="text-slate-400 text-sm">Description</p>
    </div>
</div>
```

### Badge Pattern

```html
<!-- Category -->
<span class="text-xs font-medium px-2.5 py-0.5 rounded bg-blue-900/60 text-blue-300">Category</span>

<!-- Publisher -->
<span class="text-xs font-medium px-2.5 py-0.5 rounded bg-purple-900/60 text-purple-300">Publisher</span>
```

### Button Pattern

```html
<button class="px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-all duration-300">
    Button Text
</button>
```

### Grid Layout

```html
<div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
    <!-- Cards -->
</div>
```

### Important Notes

- Always use Tailwind utility classes - no custom CSS
- Use `transition-all duration-300` for smooth animations
- Include `backdrop-blur-sm` on card overlays
- Use opacity variants like `bg-slate-800/60` for glass effects
- Font: Inter via Google Fonts
