import { describe, expect, it } from "vitest";
import { buildProduct } from "@/test/fixtures";
import { renderWithProviders, screen } from "@/test/render";
import { ProductCard, ProductCardSkeleton } from "./ProductCard";

describe("ProductCard", () => {
  it("renders the product's name, category, price and stock", () => {
    const product = buildProduct({
      name: "Wireless Mouse",
      category: "Electronics",
      price: 29.99,
      stockQuantity: 42,
    });

    renderWithProviders(<ProductCard product={product} />);

    expect(screen.getByText("Wireless Mouse")).toBeInTheDocument();
    expect(screen.getByText("Electronics")).toBeInTheDocument();
    expect(screen.getByText("$29.99")).toBeInTheDocument();
    expect(screen.getByText("42 in stock")).toBeInTheDocument();
  });

  it("shows an Inactive badge when the product is not active", () => {
    const product = buildProduct({ isActive: false });

    renderWithProviders(<ProductCard product={product} />);

    expect(screen.getByText("Inactive")).toBeInTheDocument();
  });

  it("does not show an Inactive badge when the product is active", () => {
    const product = buildProduct({ isActive: true });

    renderWithProviders(<ProductCard product={product} />);

    expect(screen.queryByText("Inactive")).not.toBeInTheDocument();
  });
});

describe("ProductCardSkeleton", () => {
  it("renders without a product", () => {
    const { container } = renderWithProviders(<ProductCardSkeleton />);

    expect(container.firstChild).not.toBeNull();
  });
});
