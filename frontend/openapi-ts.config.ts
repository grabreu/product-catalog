import { defineConfig } from "@hey-api/openapi-ts";

export default defineConfig({
  input: "http://localhost:5135/openapi/v1.json",
  output: "src/lib/api",
  plugins: [
    {
      name: "@tanstack/react-query",
      infiniteQueryKeys: false,
      infiniteQueryOptions: false,
    },
  ],
});
