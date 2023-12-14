import {createBrowserRouter, RouteObject} from "react-router-dom";
import RegisterForm from "../features/form/RegisterForm";
import HomePage from "../features/HomePage";
import App from "../app/layout/App";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: '/', element: <HomePage /> },
            { path: '/Authentication/RegisterForm', element: <RegisterForm /> }
        ]
    }
]

export const router = createBrowserRouter(routes);