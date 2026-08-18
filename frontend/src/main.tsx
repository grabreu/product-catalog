import { QueryClientProvider } from "@tanstack/react-query";
import { RouterProvider } from "@tanstack/react-router";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { ThemeProvider } from "./components/ui/theme";
import { TooltipProvider } from "./components/ui/tooltip";
import { env } from "./config/env";
import { client } from "./lib/api/client.gen";
import { queryClient } from "./lib/query/queryClient";
import { router } from "./router";

import "./index.css";

client.setConfig({ baseUrl: env.API_URL });

const rootElement = document.getElementById("root");

if (!rootElement) {
  throw new Error("Root element not found");
}

if (!rootElement.innerHTML) {
  const root = createRoot(rootElement);
  root.render(
    <StrictMode>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider>
          <TooltipProvider>
            <RouterProvider router={router} />
          </TooltipProvider>
        </ThemeProvider>
      </QueryClientProvider>
    </StrictMode>,
  );
}
