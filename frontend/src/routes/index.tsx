import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { Badge } from "@/components/ui/badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { CreateProductDialog } from "@/features/products/CreateProductDialog";
import { EditProductDialog } from "@/features/products/EditProductDialog";
import { ToggleProductActiveButton } from "@/features/products/ToggleProductActiveButton";
import { getProductsOptions } from "@/lib/api/@tanstack/react-query.gen";

export const Route = createFileRoute("/")({
  component: Index,
});

function Index() {
  const { data: page, isLoading, error } = useQuery(getProductsOptions());

  return (
    <div className="mx-auto max-w-5xl p-6">
      <div className="mb-4 flex items-center justify-between">
        <h1 className="text-lg font-semibold">Products</h1>
        <CreateProductDialog />
      </div>

      {isLoading && (
        <p className="text-sm text-muted-foreground">Loading products...</p>
      )}

      {error && (
        <p role="alert" className="text-sm text-destructive">
          {error.title ?? "Failed to load products."}
        </p>
      )}

      {page && page.items.length === 0 && (
        <p className="text-sm text-muted-foreground">
          No products yet. Create one to get started.
        </p>
      )}

      {page && page.items.length > 0 && (
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Name</TableHead>
              <TableHead>SKU</TableHead>
              <TableHead>Price</TableHead>
              <TableHead>Category</TableHead>
              <TableHead>Stock</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {page.items.map((product) => (
              <TableRow key={product.id}>
                <TableCell className="font-medium">{product.name}</TableCell>
                <TableCell className="text-muted-foreground">
                  {product.sku}
                </TableCell>
                <TableCell>${Number(product.price).toFixed(2)}</TableCell>
                <TableCell>
                  <Badge variant="secondary">{product.category}</Badge>
                </TableCell>
                <TableCell>{product.stockQuantity}</TableCell>
                <TableCell>
                  <Badge variant={product.isActive ? "default" : "outline"}>
                    {product.isActive ? "Active" : "Inactive"}
                  </Badge>
                </TableCell>
                <TableCell>
                  <div className="flex items-center justify-end gap-2">
                    <EditProductDialog product={product} />
                    <ToggleProductActiveButton product={product} />
                  </div>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}
    </div>
  );
}
