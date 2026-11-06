import { useState } from 'react';
import { api } from './api';
import { useNavigate, Link } from 'react-router-dom';

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setError('');
        try {
            const res = await api.post('/auth/login', { email, password });
            // Сохраняем данные для авторизации
            localStorage.setItem('token', res.data.token);
            localStorage.setItem('role', res.data.role);
            localStorage.setItem('username', res.data.username);
            navigate('/');
        } catch (err) {
            setError('Неверный логин или пароль');
        }
    };

    return (
        <div className="max-w-md mx-auto mt-20 bg-white p-6 rounded shadow">
            <h2 className="text-2xl font-bold mb-4">Вход</h2>
            {error && <div className="bg-red-100 text-red-700 p-2 mb-4 rounded">{error}</div>}
            <form onSubmit={handleLogin} className="flex flex-col gap-3">
                <input type="email" placeholder="Email" required className="border p-2 rounded"
                    onChange={e => setEmail(e.target.value)} />
                <input type="password" placeholder="Пароль" required className="border p-2 rounded"
                    onChange={e => setPassword(e.target.value)} />
                <button type="submit" className="bg-blue-500 text-white p-2 rounded hover:bg-blue-600">Войти</button>
            </form>
            <p className="mt-4 text-sm text-center">
                Нет аккаунта? <Link to="/register" className="text-blue-500 hover:underline">Зарегистрироваться</Link>
            </p>
        </div>
    );
}