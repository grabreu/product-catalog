import userEvent from "@testing-library/user-event";
import { HttpResponse, http } from "msw";
import { describe, expect, it } from "vitest";
import { env } from "@/config/env";
import type { UpdateProductRequest } from "@/lib/api/types.gen";
import { buildProduct } from "@/test/fixtures";
import { server } from "@/test/msw/server";
import { renderWithProviders, screen, waitFor, within } from "@/test/render";
import { ProductEditDialog } from "./ProductEditDialog";

describe("ProductEditDialog", () => {
  it("submits the edited fields to the update endpoint and closes", async () => {
    const product = buildProduct({
      id: "p1",
      name: "Wireless Mouse",
      description: "A wireless mouse.",
      category: "Electronics",
    });
    let requestBody: UpdateProductRequest | undefined;
    server.use(
      http.put(`${env.API_URL}/products/${product.id}`, async ({ request }) => {
        requestBody = (await request.json()) as UpdateProductRequest;
        return HttpResponse.json({ ...product, ...requestBody });
      }),
    );

    const user = userEvent.setup();
    renderWithProviders(<ProductEditDialog product={product} />);

    await user.click(screen.getByRole("button", { name: "Edit" }));

    const dialog = await screen.findByRole("dialog");
    const nameInput = within(dialog).getByLabelText("Name");
    await user.clear(nameInput);
    await user.type(nameInput, "Wireless Mouse Pro");
    await user.click(within(dialog).getByRole("button", { name: "Save" }));

    await waitFor(() =>
      expect(requestBody).toEqual({
        name: "Wireless Mouse Pro",
        description: "A wireless mouse.",
        category: "Electronics",
      }),
    );
    await waitFor(() =>
      expect(screen.queryByRole("dialog")).not.toBeInTheDocument(),
    );
  });

  it("shows a validation error when the name is cleared", async () => {
    const product = buildProduct({ id: "p2" });

    const user = userEvent.setup();
    renderWithProviders(<ProductEditDialog product={product} />);

    await user.click(screen.getByRole("button", { name: "Edit" }));

    const dialog = await screen.findByRole("dialog");
    const nameInput = within(dialog).getByLabelText("Name");
    await user.clear(nameInput);
    await user.click(within(dialog).getByRole("button", { name: "Save" }));

    expect(
      await within(dialog).findByText("Name is required"),
    ).toBeInTheDocument();
  });
});
