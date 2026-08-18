import { createFileRoute, Outlet } from "@tanstack/react-router";
import { SiteHeader } from "@/components/layout/SiteHeader";

const Layout = () => {
  return (
    <div className="min-h-svh bg-background">
      <SiteHeader />
      <main>
        <Outlet />
      </main>
    </div>
  );
};

export const Route = createFileRoute("/_app")({
  component: Layout,
});
