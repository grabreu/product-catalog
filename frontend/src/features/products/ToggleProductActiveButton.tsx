import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Button } from "@/components/ui/button";
import {
  deactivateProductMutation,
  getProductsQueryKey,
  reactivateProductMutation,
} from "@/lib/api/@tanstack/react-query.gen";
import type { ProductDto } from "@/lib/api/types.gen";

export function ToggleProductActiveButton({
  product,
}: {
  product: ProductDto;
}) {
  const queryClient = useQueryClient();

  const deactivate = useMutation({
    ...deactivateProductMutation(),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: getProductsQueryKey() });
    },
  });

  const reactivate = useMutation({
    ...reactivateProductMutation(),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: getProductsQueryKey() });
    },
  });

  const mutation = product.isActive ? deactivate : reactivate;

  return (
    <>
      <Button
        variant="outline"
        disabled={mutation.isPending}
        onClick={() => mutation.mutate({ path: { id: product.id } })}
      >
        {product.isActive
          ? mutation.isPending
            ? "Deactivating..."
            : "Deactivate"
          : mutation.isPending
            ? "Reactivating..."
            : "Reactivate"}
      </Button>
      {mutation.isError && (
        <span role="alert" className="ml-2 text-sm text-destructive">
          {mutation.error.title ?? "Something went wrong."}
        </span>
      )}
    </>
  );
}
