import type { ProductDto } from "@/lib/api/types.gen";

export const buildProduct = (
  overrides: Partial<ProductDto> = {},
): ProductDto => ({
  id: "b6f8e9c0-1c2a-4b3d-9e4f-000000000001",
  name: "Wireless Mouse",
  sku: "WM-100",
  description: "A wireless mouse.",
  price: 29.99,
  category: "Electronics",
  stockQuantity: 42,
  isActive: true,
  createdAt: "2026-01-01T00:00:00.000Z",
  updatedAt: "2026-01-01T00:00:00.000Z",
  ...overrides,
});
