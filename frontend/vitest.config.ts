import react from "@vitejs/plugin-react";
import { defineConfig } from "vitest/config";

export default defineConfig({
  resolve: { tsconfigPaths: true },
  plugins: [react()],
  test: {
    environment: "jsdom",
    setupFiles: ["./src/test/setup.ts"],
    css: false,
    coverage: {
      provider: "v8",
      reporter: ["lcov", "text"],
      reportsDirectory: "./coverage",
      include: ["src/**"],
      exclude: [
        "src/lib/api/**",
        "src/components/ui/**",
        "src/routeTree.gen.ts",
        "src/test/**",
        "**/*.test.{ts,tsx}",
      ],
    },
  },
});
