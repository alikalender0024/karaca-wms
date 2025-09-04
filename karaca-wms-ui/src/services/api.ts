import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5116/api", // Backend API URL
});

export default api;
