import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
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
    <ul>
      {page?.items.map((product) => (
        <li key={product.id}>
          {product.name} — {product.sku} — ${product.price} — {product.category}{" "}
          — stock: {product.stockQuantity} —{" "}
          {product.isActive ? "active" : "inactive"}
        </li>
      ))}
    </ul>
  );
}
