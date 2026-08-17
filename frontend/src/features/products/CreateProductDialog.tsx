import { useForm } from "@tanstack/react-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import * as z from "zod";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  createProductMutation,
  getProductsQueryKey,
} from "@/lib/api/@tanstack/react-query.gen";
import type { ProductCategory } from "@/lib/api/types.gen";

const FORM_ID = "create-product-form";

const categories: ProductCategory[] = [
  "Electronics",
  "Apparel",
  "Home",
  "Other",
];

const createProductSchema = z.object({
  name: z.string().min(1, "Name is required"),
  sku: z.string().min(1, "SKU is required"),
  price: z
    .string()
    .refine((value) => Number(value) > 0, "Price must be greater than 0"),
  category: z.enum(categories, "Category is required"),
});

export function CreateProductDialog() {
  const [open, setOpen] = useState(false);
  const queryClient = useQueryClient();

  const mutation = useMutation({
    ...createProductMutation(),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: getProductsQueryKey() });
      handleOpenChange(false);
    },
  });

  const form = useForm({
    defaultValues: {
      name: "",
      sku: "",
      price: "",
      category: "" as ProductCategory | "",
    },
    validators: {
      onSubmit: createProductSchema,
    },
    onSubmit: async ({ value }) => {
      const body = createProductSchema.parse(value);
      await mutation.mutateAsync({ body });
    },
  });

  function handleOpenChange(nextOpen: boolean) {
    setOpen(nextOpen);
    if (!nextOpen) {
      form.reset();
      mutation.reset();
    }
  }

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogTrigger render={<Button>New Product</Button>} />
      <DialogContent>
        <DialogHeader>
          <DialogTitle>New Product</DialogTitle>
        </DialogHeader>

        <form
          id={FORM_ID}
          onSubmit={(e) => {
            e.preventDefault();
            form.handleSubmit();
          }}
        >
          <FieldGroup>
            <form.Field name="name">
              {(field) => {
                const isInvalid =
                  field.state.meta.isTouched && !field.state.meta.isValid;
                return (
                  <Field data-invalid={isInvalid}>
                    <FieldLabel htmlFor={field.name}>Name</FieldLabel>
                    <Input
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      aria-invalid={isInvalid}
                    />
                    {isInvalid && (
                      <FieldError errors={field.state.meta.errors} />
                    )}
                  </Field>
                );
              }}
            </form.Field>

            <form.Field name="sku">
              {(field) => {
                const isInvalid =
                  field.state.meta.isTouched && !field.state.meta.isValid;
                return (
                  <Field data-invalid={isInvalid}>
                    <FieldLabel htmlFor={field.name}>SKU</FieldLabel>
                    <Input
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      aria-invalid={isInvalid}
                    />
                    {isInvalid && (
                      <FieldError errors={field.state.meta.errors} />
                    )}
                  </Field>
                );
              }}
            </form.Field>

            <form.Field name="price">
              {(field) => {
                const isInvalid =
                  field.state.meta.isTouched && !field.state.meta.isValid;
                return (
                  <Field data-invalid={isInvalid}>
                    <FieldLabel htmlFor={field.name}>Price</FieldLabel>
                    <Input
                      id={field.name}
                      name={field.name}
                      type="number"
                      step="0.01"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      aria-invalid={isInvalid}
                    />
                    {isInvalid && (
                      <FieldError errors={field.state.meta.errors} />
                    )}
                  </Field>
                );
              }}
            </form.Field>

            <form.Field name="category">
              {(field) => {
                const isInvalid =
                  field.state.meta.isTouched && !field.state.meta.isValid;
                return (
                  <Field data-invalid={isInvalid}>
                    <FieldLabel htmlFor={field.name}>Category</FieldLabel>
                    <Select
                      value={field.state.value}
                      onValueChange={(value) =>
                        field.handleChange(value as ProductCategory)
                      }
                    >
                      <SelectTrigger id={field.name} onBlur={field.handleBlur}>
                        <SelectValue placeholder="Select a category" />
                      </SelectTrigger>
                      <SelectContent>
                        {categories.map((category) => (
                          <SelectItem key={category} value={category}>
                            {category}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                    {isInvalid && (
                      <FieldError errors={field.state.meta.errors} />
                    )}
                  </Field>
                );
              }}
            </form.Field>
          </FieldGroup>

          {mutation.isError && (
            <p role="alert" className="mt-4 text-sm text-destructive">
              {mutation.error.detail ?? mutation.error.title}
            </p>
          )}
        </form>

        <DialogFooter showCloseButton>
          <Button type="submit" form={FORM_ID} disabled={mutation.isPending}>
            {mutation.isPending ? "Creating..." : "Create"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
