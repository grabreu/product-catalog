import type { ProductDto } from "@/lib/api/types.gen";
import { ProductCard, ProductCardSkeleton } from "../ProductCard";

export type ProductListProps = {
  products: ProductDto[];
  isLoading?: boolean;
};

export const ProductList = ({ products, isLoading }: ProductListProps) => {
  if (isLoading) {
    return <ProductListSkeleton />;
  }

  if (!products || products.length === 0) {
    return null;
  }

  return (
    <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
      {products.map((product) => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  );
};

export type ProductListSkeletonProps = {
  count?: number;
};

export const ProductListSkeleton = ({
  count = 10,
}: ProductListSkeletonProps) => {
  return (
    <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
      {Array.from({ length: count }).map((_, index) => (
        // biome-ignore lint/suspicious/noArrayIndexKey: This is a skeleton component, so using the index as a key is acceptable here.
        <ProductCardSkeleton key={index} />
      ))}
    </div>
  );
};
