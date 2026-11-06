import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import ValeraList from './ValeraList';
import ValeraStats from './ValeraStats';
import Login from './Login';
import Register from './Register';

function ProtectedRoute({ children }) {
    const token = localStorage.getItem('token');
    return token ? children : <Navigate to="/login" />;
}

function App() {
    return (
        <BrowserRouter>
            <div className="min-h-screen bg-gray-100 p-8">
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    
                    <Route path="/" element={<ProtectedRoute><ValeraList /></ProtectedRoute>} />
                    <Route path="/valera/:id" element={<ProtectedRoute><ValeraStats /></ProtectedRoute>} />
                </Routes>
            </div>
        </BrowserRouter>
    );
}
export default App;