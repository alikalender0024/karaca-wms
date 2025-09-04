import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Products from "./pages/Products";
import './App.css';

function App() {
  return (
    <Router>
      <div>
        <nav>
          <ul>
            <li><Link to="/products">Products</Link></li>
          </ul>
        </nav>
      </div>
      <Routes>
        <Route path="/products" element={<Products />} />
        <Route path="*" element={<div>Hoşgeldiniz! Sol menüden Products sayfasına gidin.</div>} />
      </Routes>
    </Router>
  );
}

export default App;
