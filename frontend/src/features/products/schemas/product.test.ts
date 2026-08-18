import { describe, expect, it } from "vitest";
import { createProductFormSchema, editProductFormSchema } from "./product";

describe("createProductFormSchema", () => {
  const validInput = {
    name: "Wireless Mouse",
    sku: "WM-100",
    price: "29.99",
    category: "Electronics",
  };

  it("accepts a fully valid submission", () => {
    const result = createProductFormSchema.safeParse(validInput);

    expect(result.success).toBe(true);
  });

  it("rejects an empty name", () => {
    const result = createProductFormSchema.safeParse({
      ...validInput,
      name: "",
    });

    expect(result.success).toBe(false);
  });

  it("rejects an empty SKU", () => {
    const result = createProductFormSchema.safeParse({
      ...validInput,
      sku: "",
    });

    expect(result.success).toBe(false);
  });

  it.each(["0", "-5", "not-a-number"])("rejects a price of %s", (price) => {
    const result = createProductFormSchema.safeParse({
      ...validInput,
      price,
    });

    expect(result.success).toBe(false);
  });

  it("accepts a price greater than 0", () => {
    const result = createProductFormSchema.safeParse({
      ...validInput,
      price: "0.01",
    });

    expect(result.success).toBe(true);
  });

  it("rejects a category outside the fixed enum", () => {
    const result = createProductFormSchema.safeParse({
      ...validInput,
      category: "Groceries",
    });

    expect(result.success).toBe(false);
  });
});

describe("editProductFormSchema", () => {
  const validInput = {
    name: "Wireless Mouse",
    description: "A wireless mouse.",
    category: "Electronics",
  };

  it("accepts a fully valid submission", () => {
    const result = editProductFormSchema.safeParse(validInput);

    expect(result.success).toBe(true);
  });

  it("accepts an empty description", () => {
    const result = editProductFormSchema.safeParse({
      ...validInput,
      description: "",
    });

    expect(result.success).toBe(true);
  });

  it("rejects an empty name", () => {
    const result = editProductFormSchema.safeParse({
      ...validInput,
      name: "",
    });

    expect(result.success).toBe(false);
  });

  it("rejects a name longer than 200 characters", () => {
    const result = editProductFormSchema.safeParse({
      ...validInput,
      name: "a".repeat(201),
    });

    expect(result.success).toBe(false);
  });

  it("rejects a description longer than 2000 characters", () => {
    const result = editProductFormSchema.safeParse({
      ...validInput,
      description: "a".repeat(2001),
    });

    expect(result.success).toBe(false);
  });

  it("rejects a category outside the fixed enum", () => {
    const result = editProductFormSchema.safeParse({
      ...validInput,
      category: "Groceries",
    });

    expect(result.success).toBe(false);
  });
});
