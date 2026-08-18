import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { ProductCreateDialog } from "@/features/products/components/ProductCreateDialog";
import { ProductList } from "@/features/products/components/ProductList";
import { getProductsOptions } from "@/lib/api/@tanstack/react-query.gen";

const ListProductsPage = () => {
  const productsQuery = useQuery(getProductsOptions());
  const products = productsQuery.data?.items ?? [];

  return (
    <div className="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
      <div className="mb-6 flex items-center justify-between gap-4">
        <h1 className="text-xl font-semibold">Products</h1>
        <ProductCreateDialog />
      </div>

      {productsQuery.error && (
        <p role="alert" className="mb-4 text-sm text-destructive">
          {productsQuery.error.title ?? "Failed to load products."}
        </p>
      )}

      {productsQuery.data && products.length === 0 ? (
        <p className="text-sm text-muted-foreground">
          No products yet. Create one to get started.
        </p>
      ) : (
        <ProductList products={products} isLoading={productsQuery.isLoading} />
      )}
    </div>
  );
};

export const Route = createFileRoute("/_app/")({
  component: ListProductsPage,
});
