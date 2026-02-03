# 🎨 ExpenseTracker Figma Design Guide

## 📋 **Getting Started in Figma**

### **1. Create New Figma File**
- File name: "ExpenseTracker - Web Application"
- Add these artboard sizes:
  - **Desktop**: 1440px × 1024px (Main design)
  - **Tablet**: 768px × 1024px (iPad)
  - **Mobile**: 375px × 812px (iPhone)

### **2. Set Up Design Tokens**

#### **Create Color Styles**
```
Primary Colors:
- Primary/Blue: #3B82F6
- Primary/Dark: #1D4ED8
- Primary/Light: #DBEAFE

Semantic Colors:
- Success: #10B981
- Warning: #F59E0B
- Danger: #EF4444
- Info: #06B6D4

Grayscale:
- Gray/50: #F9FAFB
- Gray/100: #F3F4F6
- Gray/200: #E5E7EB
- Gray/400: #9CA3AF
- Gray/600: #4B5563
- Gray/900: #111827
```

#### **Typography Styles**
```
Heading/H1: Inter Bold 30px
Heading/H2: Inter Semibold 24px
Heading/H3: Inter Semibold 20px
Body/Large: Inter Regular 18px
Body/Medium: Inter Regular 16px
Body/Small: Inter Regular 14px
Caption: Inter Regular 12px
```

### **3. Component Library Setup**

#### **Create Master Components**

**🔘 Buttons**
- Primary Button (240px × 48px)
  - Background: Primary/Blue
  - Text: White, Body/Medium
  - Border radius: 8px
  - Padding: 12px 24px

- Secondary Button
  - Background: Transparent
  - Border: 1px Gray/200
  - Text: Gray/900, Body/Medium

- Icon Button (48px × 48px)
  - Square, icon only
  - Hover states

**📄 Cards**
- Base Card (auto × auto)
  - Background: White
  - Shadow: 0px 4px 6px rgba(0,0,0,0.1)
  - Border radius: 12px
  - Padding: 24px

- Metric Card (280px × 120px)
  - Title, value, percentage change
  - Icon in top-right
  - Color-coded trends

**📊 Charts**
- Chart Container (560px × 320px)
- Use Figma plugins:
  - Chart by Figma
  - Chartify
  - Google Sheets Sync

**📝 Form Elements**
- Text Input (320px × 48px)
  - Border: 1px Gray/200
  - Focus: 2px Primary/Blue
  - Placeholder text

- Select Dropdown
- Date Picker
- Amount Input with currency

### **4. Page Layout Templates**

#### **Master Layout (1440px × 1024px)**
```
┌─────────────────────────────────────────────────────────────┐
│ Sidebar (240px) │              Main Content                 │
│                 │                                           │
│ ┌─────────────┐ │ ┌─────────────────────────────────────┐   │
│ │ Navigation  │ │ │ Top Bar (64px height)               │   │
│ │ Menu        │ │ └─────────────────────────────────────┘   │
│ │             │ │                                           │
│ │ - Dashboard │ │ ┌─────────────────────────────────────┐   │
│ │ - Expenses  │ │ │                                     │   │
│ │ - Categories│ │ │         Page Content                │   │
│ │ - Budgets   │ │ │         (960px wide)                │   │
│ │ - Goals     │ │ │                                     │   │
│ │ - Analytics │ │ │                                     │   │
│ │             │ │ │                                     │   │
│ └─────────────┘ │ └─────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

### **5. Key Pages to Design**

#### **🏠 Dashboard Page**
**Components to create:**
- Financial metrics cards (4 across)
- Quick action buttons (2×2 grid)
- Spending trends chart
- Expenses by category pie chart
- Recent transactions list

#### **💰 Expenses Page**
**Components to create:**
- Page header with filters
- Add expense button (prominent)
- Expense table/list view
- Filter chips
- Pagination controls

#### **📁 Categories Page**
**Components to create:**
- Category grid cards (4 across)
- Add category modal
- Category card with icon, name, total, count
- Edit/delete actions

#### **🎯 Budgets Page**
**Components to create:**
- Budget progress cards
- Progress bars with percentages
- Status indicators (on track, warning, exceeded)
- Monthly/category filters

#### **💎 Savings Goals Page**
**Components to create:**
- Goal cards with progress
- Target date indicators
- Contribute action buttons
- Achievement celebrations

#### **📈 Analytics Page**
**Components to create:**
- Date range selector
- Multiple chart containers
- Comparison views
- Export/report buttons

### **6. Design System Best Practices**

#### **Spacing Guidelines**
```
Use 8px grid system:
- 8px: Small gaps, icon spacing
- 16px: Standard padding, between elements
- 24px: Card padding, section spacing
- 32px: Large section gaps
- 48px: Page margins
```

#### **Icon System**
- Use Lucide React icon set for consistency
- Size: 16px, 20px, 24px
- Color: Inherit from parent or Gray/600

#### **States & Interactions**
- Hover states for all interactive elements
- Loading states for buttons and data
- Empty states for lists
- Error states for forms

### **7. Figma Plugins to Install**

**Essential Plugins:**
- **Iconify**: For icons (Lucide React set)
- **Unsplash**: For placeholder images
- **Content Reel**: For realistic data
- **Chart**: For creating data visualizations
- **Auto Layout**: For responsive designs

**Nice to Have:**
- **Stark**: For accessibility checking
- **Color Oracle**: For colorblind testing
- **Figma to React**: For developer handoff

### **8. Prototyping Guidelines**

#### **Create Interactive Prototype**
- Connect key user flows:
  - Login → Dashboard
  - Dashboard → Add Expense
  - Expenses → Edit/Delete
  - Budgets → Create/Edit
  - Goals → Contribute

#### **Micro-interactions**
- Button hover effects
- Loading animations
- Success/error toast notifications
- Modal open/close animations
- Chart hover tooltips

### **9. Developer Handoff Prep**

#### **Organize Layers**
- Use clear naming conventions
- Group related elements
- Create component variants for different states

#### **Add Documentation**
- Component descriptions
- Interaction notes
- Copy for empty states
- Error message text

#### **Export Assets**
- SVG icons
- Image assets in 1x, 2x, 3x
- Component specs

### **10. Mobile Design Considerations**

#### **Mobile Layout (375px)**
```
┌─────────────────┐
│ ☰ Header    🔔  │
├─────────────────┤
│                 │
│ Stack cards     │
│ vertically      │
│                 │
│ Full-width      │
│ components      │
│                 │
│ Larger touch    │
│ targets (44px)  │
│                 │
├─────────────────┤
│ Bottom Tab Nav  │
│ 📊 💰 📁 🎯 📈  │
└─────────────────┘
```

#### **Mobile Specific Components**
- Bottom navigation bar
- Slide-out drawer menu
- Swipe gestures for tables
- Pull-to-refresh
- Touch-friendly forms

This guide will help you create a comprehensive, professional ExpenseTracker design in Figma. Start with the design system, then build components, and finally create the full page layouts.