import { PackageIcon } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { Card, CardContent } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import type { ProductDto } from "@/lib/api/types.gen";
import { ProductEditDialog } from "../ProductEditDialog";
import { ProductStatusToggle } from "../ProductStatusToggle";

export type ProductCardProps = {
  product: ProductDto;
};

export const ProductCard = ({ product }: ProductCardProps) => {
  return (
    <Card className="group/product-card gap-0 py-0">
      <div className="relative flex aspect-square items-center justify-center bg-muted">
        <PackageIcon
          className="size-10 text-muted-foreground/40"
          strokeWidth={1.5}
        />

        {!product.isActive && (
          <Badge
            variant="outline"
            className="absolute top-2 left-2 bg-background"
          >
            Inactive
          </Badge>
        )}

        <div className="absolute top-2 right-2 flex gap-1 opacity-0 transition-opacity group-hover/product-card:opacity-100">
          <ProductEditDialog product={product} />
          <ProductStatusToggle product={product} />
        </div>
      </div>

      <CardContent className="flex flex-col gap-1 py-3">
        <span className="text-xs text-muted-foreground">
          {product.category}
        </span>
        <h3 className="line-clamp-1 text-sm font-medium">{product.name}</h3>
        <div className="mt-1 flex items-center justify-between">
          <span className="text-base font-semibold">
            ${Number(product.price).toFixed(2)}
          </span>
          <span className="text-xs text-muted-foreground">
            {product.stockQuantity} in stock
          </span>
        </div>
      </CardContent>
    </Card>
  );
};

export const ProductCardSkeleton = () => {
  return (
    <Card className="gap-0 py-0">
      <Skeleton className="aspect-square w-full rounded-none" />
      <CardContent className="flex flex-col gap-2 py-3">
        <Skeleton className="h-3 w-16" />
        <Skeleton className="h-4 w-3/4" />
        <Skeleton className="h-4 w-1/2" />
      </CardContent>
    </Card>
  );
};
