import { describe, expect, it } from "vitest";
import { buildProduct } from "@/test/fixtures";
import { renderWithProviders, screen } from "@/test/render";
import { ProductList } from "./ProductList";

describe("ProductList", () => {
  it("renders a card for each product", () => {
    const products = [
      buildProduct({ id: "1", name: "Wireless Mouse" }),
      buildProduct({ id: "2", name: "Mechanical Keyboard" }),
    ];

    renderWithProviders(<ProductList products={products} />);

    expect(screen.getByText("Wireless Mouse")).toBeInTheDocument();
    expect(screen.getByText("Mechanical Keyboard")).toBeInTheDocument();
  });

  it("renders skeletons while loading, instead of the products", () => {
    const products = [buildProduct({ name: "Wireless Mouse" })];

    const { container } = renderWithProviders(
      <ProductList products={products} isLoading />,
    );

    expect(screen.queryByText("Wireless Mouse")).not.toBeInTheDocument();
    expect(
      container.querySelectorAll('[data-slot="skeleton"]').length,
    ).toBeGreaterThan(0);
  });

  it("renders nothing when there are no products", () => {
    const { container } = renderWithProviders(<ProductList products={[]} />);

    expect(container).toBeEmptyDOMElement();
  });
});
