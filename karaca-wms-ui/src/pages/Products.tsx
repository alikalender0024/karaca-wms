import { useEffect, useState } from "react";
import type { Product } from "../types/Product";
import { getProducts, postProduct } from "../services/productService";


const Products: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  // Yeni ürün form state
  const [newProduct, setNewProduct] = useState({
    sku: "",
    name: "",
    unitSize: "",
    weight: 0,
  });

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const data = await getProducts();
        setProducts(data);
      } catch (error) {
        console.error("Ürünler yüklenirken hata oluştu:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchProducts();
  }, []);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewProduct({ ...newProduct, [e.target.name]: e.target.value });
  };


const handleAddProduct = async () => {
  try {
    const added = await postProduct(newProduct); // Backend’e gönder
    setProducts([...products, added]);           // Listeye ekle
    setNewProduct({ sku: "", name: "", unitSize: "", weight: 0 }); // Formu sıfırla
  } catch (error) {
    console.error("Ürün eklenirken hata oluştu:", error);
  }
};

  if (loading) return <div>Yükleniyor...</div>;

  return (
    <div>
      <h1>Products</h1>

      <div>
        <h2>Yeni Ürün Ekle</h2>
        <input
          name="sku"
          placeholder="SKU"
          value={newProduct.sku}
          onChange={handleInputChange}
        />
        <input
          name="name"
          placeholder="Name"
          value={newProduct.name}
          onChange={handleInputChange}
        />
        <input
          name="unitSize"
          placeholder="Unit Size"
          value={newProduct.unitSize}
          onChange={handleInputChange}
        />
        <input
          name="weight"
          type="number"
          placeholder="Weight"
          value={newProduct.weight}
          onChange={handleInputChange}
        />
        <button onClick={handleAddProduct}>Ekle</button>
      </div>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>SKU</th>
            <th>Name</th>
            <th>Unit Size</th>
            <th>Weight</th>
          </tr>
        </thead>
        <tbody>
          {products.map((p) => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.sku}</td>
              <td>{p.name}</td>
              <td>{p.unitSize}</td>
              <td>{p.weight}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Products;
