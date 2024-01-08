import { createBrowserRouter, RouteObject } from "react-router-dom";
import RegisterForm from "../../features/form/RegisterForm";
import HomePage from "../../features/HomePage";
import Blog from "../../features/Blog";
import Photos from "../../features/Photos";
import App from "../layout/App";
import Contact from "../../features/Contact";

export const routes: RouteObject[] = [
    {
        path: "/",
        element: <App />,
        children: [
            { path: "/", element: <HomePage /> },
            { path: "/Authentication/RegisterForm", element: <RegisterForm /> },
            { path: "/Blog", element: <Blog /> },
            { path: "/Photos", element: <Photos /> },
            { path: "/Contact", element: <Contact /> },
        ],
    },
];

export const router = createBrowserRouter(routes);
