import { useMutation, useQueryClient } from "@tanstack/react-query";
import { EyeIcon, EyeOffIcon } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from "@/components/ui/tooltip";
import {
  deactivateProductMutation,
  getProductsQueryKey,
  reactivateProductMutation,
} from "@/lib/api/@tanstack/react-query.gen";
import type { ProductDto } from "@/lib/api/types.gen";

export type ProductStatusToggleProps = {
  product: ProductDto;
};

export const ProductStatusToggle = ({ product }: ProductStatusToggleProps) => {
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
  const label = product.isActive ? "Deactivate" : "Reactivate";

  return (
    <Tooltip>
      <TooltipTrigger
        render={
          <Button
            variant="secondary"
            size="icon-sm"
            className="bg-background/80 backdrop-blur-sm"
            disabled={mutation.isPending}
            onClick={() => mutation.mutate({ path: { id: product.id } })}
          >
            {product.isActive ? <EyeIcon /> : <EyeOffIcon />}
            <span className="sr-only">{label}</span>
          </Button>
        }
      />
      <TooltipContent>{label}</TooltipContent>
    </Tooltip>
  );
};
