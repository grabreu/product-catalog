import z from "zod";
import { PRODUCT_CATEGORIES } from "../constants/product";

export const createProductFormSchema = z.object({
  name: z.string().min(1, "Name is required"),
  sku: z.string().min(1, "SKU is required"),
  price: z
    .string()
    .refine((value) => Number(value) > 0, "Price must be greater than 0"),
  category: z.enum(PRODUCT_CATEGORIES, "Category is required"),
});

export type CreateProductFormSchema = z.infer<typeof createProductFormSchema>;

export const editProductFormSchema = z.object({
  name: z
    .string()
    .min(1, "Name is required")
    .max(200, "Name must be at most 200 characters"),
  description: z
    .string()
    .max(2000, "Description must be at most 2000 characters"),
  category: z.enum(PRODUCT_CATEGORIES, "Category is required"),
});

export type EditProductFormSchema = z.infer<typeof editProductFormSchema>;
