import React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import MenuIcon from "@mui/icons-material/Menu";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import AdbIcon from "@mui/icons-material/Adb";
import { Link } from "react-router-dom";

export const NavBar = () => {
    return (
        <AppBar position="static">
            <Container maxWidth="xl">
                <Toolbar
                    style={{ display: "flex", justifyContent: "space-between" }}
                >
                    <Typography variant="h6" component="div">
                        Josiah
                    </Typography>
                    <div
                        style={{
                            display: "flex",
                            justifyContent: "space-around",
                            width: "75%",
                        }}
                    >
                        <Button component={Link} to="/" color="inherit">
                            Home
                        </Button>
                        <Button component={Link} to="Blog" color="inherit">
                            Blog
                        </Button>
                        <Button component={Link} to="Photos" color="inherit">
                            Photos
                        </Button>
                        <Button component={Link} to="contact" color="inherit">
                            Contact
                        </Button>
                    </div>
                </Toolbar>
            </Container>
        </AppBar>
    );
};

export default NavBar;
