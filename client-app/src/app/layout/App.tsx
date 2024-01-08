import Container from "@mui/material/Container";
import { Routes, Route } from "react-router-dom";
import HomePage from "../../features/HomePage";
import RegisterForm from "../../features/form/RegisterForm";
import { ToastContainer } from "react-toastify";
import { NavBar } from "./NavBar";
import Blog from "../../features/Blog";
import "react-toastify/dist/ReactToastify.css";
import Photos from "../../features/Photos";
import Contact from "../../features/Contact";

function App() {
    return (
        <>
            <>
                <ToastContainer />
                <Container style={{ marginTop: "7em" }}>
                    <NavBar />
                    <Routes>
                        <Route path="/" element={<HomePage />} />
                        <Route
                            path="/Authentication/RegisterForm"
                            element={<RegisterForm />}
                        />
                        <Route path="/Blog" element={<Blog />} />
                        <Route path="/Photos" element={<Photos />} />
                        <Route path="/Contact" element={<Contact />} />
                        {/* Add more routes as needed */}
                    </Routes>
                </Container>
            </>
        </>
    );
}

export default App;
