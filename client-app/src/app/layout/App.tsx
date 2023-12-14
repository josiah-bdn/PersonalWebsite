import Container from "@mui/material/Container";
import { Routes, Route } from 'react-router-dom';
import HomePage from '../../features/HomePage';
import RegisterForm from '../../features/form/RegisterForm'


function App() {
  
  return (
    <>
      <>
      <Container style={{ marginTop: '7em' }}>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/Authentication/RegisterForm" element={<RegisterForm />} />
        {/* Add more routes as needed */}
      </Routes>
    </Container>
      </>
    </>
  );
}

export default App;
