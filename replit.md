# Quanta Shop Web

A Nuxt.js 3 e-commerce frontend for Quanta Shop, a Brazilian cashback shopping platform.

## Tech Stack

- **Framework:** Nuxt.js 3 (Vue 3, TypeScript)
- **CSS:** Bootstrap 5 + SCSS
- **State Management:** Pinia
- **UI Components:** Swiper, Vue3 Carousel, VeeValidate
- **PWA Support:** @vite-pwa/nuxt
- **Rendering:** SSR disabled (client-side SPA)

## Project Structure

- `pages/` - All application pages/routes
- `components/` - Reusable Vue components
- `layouts/` - Page layout wrappers (default, layout-one through layout-four)
- `composables/` - Shared Vue composition functions (useApi, useMask, useSticky)
- `pinia/` - Pinia store modules
- `data/` - Static data files (products, blogs, categories, etc.)
- `services/` - API service modules
- `plugins/` - Nuxt plugins (directives, filters, mask)
- `assets/` - CSS/SCSS styles and fonts
- `public/` - Static assets (images, icons)

## Development

```bash
npm run dev     # Start dev server on port 5000
npm run build   # Build for production
npm run generate # Generate static site
```

## Configuration

- Dev server runs on `0.0.0.0:5000` (configured in `nuxt.config.ts`)
- API base URL set via `NUXT_API_BASE_URL` environment variable
- Production env file: `.env.production`

## Deployment

Configured as a static site deployment:
- Build command: `npm run generate`
- Output directory: `.output/public`
