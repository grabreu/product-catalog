import userEvent from "@testing-library/user-event";
import { HttpResponse, http } from "msw";
import { describe, expect, it, vi } from "vitest";
import { env } from "@/config/env";
import { buildProduct } from "@/test/fixtures";
import { server } from "@/test/msw/server";
import { renderWithProviders, screen, waitFor } from "@/test/render";
import { ProductStatusToggle } from "./ProductStatusToggle";

describe("ProductStatusToggle", () => {
  it("calls the deactivate endpoint when the product is active", async () => {
    const product = buildProduct({ id: "p1", isActive: true });
    const deactivate = vi.fn();
    server.use(
      http.post(`${env.API_URL}/products/${product.id}/deactivate`, () => {
        deactivate();
        return HttpResponse.json({ ...product, isActive: false });
      }),
    );

    const user = userEvent.setup();
    renderWithProviders(<ProductStatusToggle product={product} />);

    await user.click(screen.getByRole("button", { name: "Deactivate" }));

    await waitFor(() => expect(deactivate).toHaveBeenCalledTimes(1));
  });

  it("calls the reactivate endpoint when the product is inactive", async () => {
    const product = buildProduct({ id: "p2", isActive: false });
    const reactivate = vi.fn();
    server.use(
      http.post(`${env.API_URL}/products/${product.id}/reactivate`, () => {
        reactivate();
        return HttpResponse.json({ ...product, isActive: true });
      }),
    );

    const user = userEvent.setup();
    renderWithProviders(<ProductStatusToggle product={product} />);

    await user.click(screen.getByRole("button", { name: "Reactivate" }));

    await waitFor(() => expect(reactivate).toHaveBeenCalledTimes(1));
  });

  it("disables the button while the mutation is pending", async () => {
    const product = buildProduct({ id: "p3", isActive: true });
    server.use(
      http.post(
        `${env.API_URL}/products/${product.id}/deactivate`,
        async () => {
          await new Promise((resolve) => setTimeout(resolve, 20));
          return HttpResponse.json({ ...product, isActive: false });
        },
      ),
    );

    const user = userEvent.setup();
    renderWithProviders(<ProductStatusToggle product={product} />);

    const button = screen.getByRole("button", { name: "Deactivate" });
    await user.click(button);

    expect(button).toBeDisabled();
    await waitFor(() => expect(button).not.toBeDisabled());
  });
});
