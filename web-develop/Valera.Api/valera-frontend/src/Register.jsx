import { useState } from 'react';
import { api } from './api';
import { useNavigate, Link } from 'react-router-dom';

export default function Register() {
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        setError('');
        try {
            await api.post('/auth/register', { email, username, password });
            alert('Регистрация успешна! Теперь войдите.');
            navigate('/login');
        } catch (err) {
            setError(err.response?.data?.error || 'Ошибка регистрации');
        }
    };

    return (
        <div className="max-w-md mx-auto mt-20 bg-white p-6 rounded shadow">
            <h2 className="text-2xl font-bold mb-4">Регистрация</h2>
            {error && <div className="bg-red-100 text-red-700 p-2 mb-4 rounded">{error}</div>}
            <form onSubmit={handleRegister} className="flex flex-col gap-3">
                <input type="text" placeholder="Имя пользователя" required className="border p-2 rounded"
                    onChange={e => setUsername(e.target.value)} />
                <input type="email" placeholder="Email" required className="border p-2 rounded"
                    onChange={e => setEmail(e.target.value)} />
                <input type="password" placeholder="Пароль" required className="border p-2 rounded"
                    onChange={e => setPassword(e.target.value)} />
                <button type="submit" className="bg-green-500 text-white p-2 rounded hover:bg-green-600">Создать аккаунт</button>
            </form>
            <p className="mt-4 text-sm text-center">
                Уже есть аккаунт? <Link to="/login" className="text-blue-500 hover:underline">Войти</Link>
            </p>
        </div>
    );
}