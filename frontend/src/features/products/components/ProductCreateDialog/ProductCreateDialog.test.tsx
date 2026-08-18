import userEvent from "@testing-library/user-event";
import { HttpResponse, http } from "msw";
import { describe, expect, it } from "vitest";
import { env } from "@/config/env";
import type { CreateProductRequest } from "@/lib/api/types.gen";
import { buildProduct } from "@/test/fixtures";
import { server } from "@/test/msw/server";
import { renderWithProviders, screen, waitFor, within } from "@/test/render";
import { ProductCreateDialog } from "./ProductCreateDialog";

describe("ProductCreateDialog", () => {
  it("submits the filled-in fields to the create endpoint and closes", async () => {
    let requestBody: CreateProductRequest | undefined;
    server.use(
      http.post(`${env.API_URL}/products`, async ({ request }) => {
        requestBody = (await request.json()) as CreateProductRequest;
        return HttpResponse.json(
          buildProduct({ ...requestBody, id: "new-id" }),
          { status: 201 },
        );
      }),
    );

    const user = userEvent.setup();
    renderWithProviders(<ProductCreateDialog />);

    await user.click(screen.getByRole("button", { name: "New Product" }));

    const dialog = await screen.findByRole("dialog");
    await user.type(within(dialog).getByLabelText("Name"), "Wireless Mouse");
    await user.type(within(dialog).getByLabelText("SKU"), "WM-100");
    await user.type(within(dialog).getByLabelText("Price"), "29.99");
    await user.click(within(dialog).getByLabelText("Category"));
    await user.click(
      await screen.findByRole("option", { name: "Electronics" }),
    );
    await user.click(within(dialog).getByRole("button", { name: "Save" }));

    await waitFor(() =>
      expect(requestBody).toEqual({
        name: "Wireless Mouse",
        sku: "WM-100",
        price: "29.99",
        category: "Electronics",
      }),
    );
    await waitFor(() =>
      expect(screen.queryByRole("dialog")).not.toBeInTheDocument(),
    );
  });

  it("shows validation errors when required fields are left empty", async () => {
    const user = userEvent.setup();
    renderWithProviders(<ProductCreateDialog />);

    await user.click(screen.getByRole("button", { name: "New Product" }));

    const dialog = await screen.findByRole("dialog");
    await user.click(within(dialog).getByRole("button", { name: "Save" }));

    expect(
      await within(dialog).findByText("Name is required"),
    ).toBeInTheDocument();
    expect(within(dialog).getByText("SKU is required")).toBeInTheDocument();
  });
});
