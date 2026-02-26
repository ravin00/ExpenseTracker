# SpendWise Frontend

React + TypeScript frontend for SpendWise.

## Stack
- React 19
- TanStack Router
- TanStack Query
- Zustand
- Tailwind CSS
- Vite

## Prerequisites
- Node.js 20+
- pnpm 10+

## Run locally
```bash
pnpm install
pnpm dev
```

The app expects the API gateway at `http://localhost:5100` by default.
Override with:

```bash
VITE_API_URL=http://localhost:5100
```

## Build
```bash
pnpm build
```

## Lint
```bash
pnpm lint
```

## Key folders
- `src/routes`: route definitions
- `src/features`: feature modules
- `src/components`: shared UI and layout
- `src/lib`: API client and helpers
- `src/stores`: Zustand stores

## Notes
Some feature modules are currently placeholders and should be removed or implemented before production release.
