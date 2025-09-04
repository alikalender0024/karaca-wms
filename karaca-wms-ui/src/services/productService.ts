import api from "../services/api";
import type { Product } from "../types/Product";

// GET fonksiyonu zaten vardı
export const getProducts = async (): Promise<Product[]> => {
  const response = await api.get<Product[]>("/products");
  return response.data;
};

// POST: Yeni ürün ekleme
export const postProduct = async (
  product: Omit<Product, "id">
): Promise<Product> => {
  const response = await api.post<Product>("/products", product);
  return response.data;
};
