import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { CreateProductDialog } from "@/features/products/CreateProductDialog";
import { EditProductDialog } from "@/features/products/EditProductDialog";
import { getProductsOptions } from "@/lib/api/@tanstack/react-query.gen";

export const Route = createFileRoute("/")({
  component: Index,
});

function Index() {
  const { data: page, isLoading, error } = useQuery(getProductsOptions());

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error.title}</div>;
  }

  return (
    <div>
      <CreateProductDialog />
      <ul>
        {page?.items.map((product) => (
          <li key={product.id}>
            {product.name} — {product.sku} — ${product.price} —{" "}
            {product.category} — stock: {product.stockQuantity} —{" "}
            {product.isActive ? "active" : "inactive"} —{" "}
            <EditProductDialog product={product} />
          </li>
        ))}
      </ul>
    </div>
  );
}
