import { useEffect, useState } from 'react';
import { api } from './api';
import { useNavigate } from 'react-router-dom';

export default function ValeraList() {
    const [valeras, setValeras] = useState([]);
    const [search, setSearch] = useState('');
    const [showForm, setShowForm] = useState(false);
    const [form, setForm] = useState({ name: '', health: 100, mana: 0 });
    const navigate = useNavigate();

    // Достаем данные пользователя из localStorage
    const role = localStorage.getItem('role');
    const username = localStorage.getItem('username');

    const loadValeras = () => {
        // Если Админ - грузим всех, если Юзер - только своих
        const endpoint = role === 'Admin' ? '/valera' : '/valera/my';
        api.get(endpoint)
           .then(res => setValeras(res.data))
           .catch(err => console.error(err));
    };

    useEffect(() => { 
        loadValeras(); 
    }, []);

    const handleCreate = async (e) => {
        e.preventDefault();
        try {
            await api.post('/valera', form);
            setShowForm(false);
            loadValeras(); // Обновляем список после создания
        } catch (err) {
            alert('Ошибка при создании Валеры');
        }
    };

    const handleLogout = () => {
        localStorage.clear(); // Удаляем токен и данные пользователя
        navigate('/login');
    };

    const filtered = valeras.filter(v => v.name.toLowerCase().includes(search.toLowerCase()));

    return (
        <div className="max-w-2xl mx-auto">
            {/* Шапка с информацией о пользователе */}
            <div className="flex justify-between items-center mb-6 bg-white p-4 rounded shadow">
                <div>
                    <span className="font-bold">{username}</span> 
                    <span className="ml-2 text-sm bg-blue-100 text-blue-800 px-2 py-1 rounded">{role}</span>
                </div>
                <button onClick={handleLogout} className="text-red-500 hover:underline">
                    Выйти
                </button>
            </div>

            <h1 className="text-3xl font-bold mb-4">
                {role === 'Admin' ? 'Все Валеры (Админ)' : 'Мои Валеры'}
            </h1>
            
            <div className="flex gap-4 mb-4">
                <input 
                    type="text" placeholder="Поиск по имени..." 
                    className="border p-2 rounded w-full"
                    value={search} onChange={e => setSearch(e.target.value)}
                />
                <button onClick={() => setShowForm(!showForm)} className="bg-blue-500 text-white px-4 py-2 rounded whitespace-nowrap">
                    Создать Валеру
                </button>
            </div>

            {showForm && (
                <form onSubmit={handleCreate} className="bg-white p-4 mb-4 rounded shadow flex flex-col gap-2">
                    <input type="text" placeholder="Имя" required className="border p-2"
                        onChange={e => setForm({...form, name: e.target.value})} />
                    <div className="flex gap-2">
                        <input type="number" placeholder="Здоровье" min="0" max="100" className="border p-2 w-1/2"
                            onChange={e => setForm({...form, health: Number(e.target.value)})} />
                        <input type="number" placeholder="Алкоголь" min="0" max="100" className="border p-2 w-1/2"
                            onChange={e => setForm({...form, mana: Number(e.target.value)})} />
                    </div>
                    <button type="submit" className="bg-green-500 text-white p-2 rounded">Сохранить</button>
                </form>
            )}

            <ul className="flex flex-col gap-2">
                {filtered.length === 0 && <p className="text-gray-500">Валеры не найдены.</p>}
                {filtered.map(v => (
                    <li key={v.id} onClick={() => navigate(`/valera/${v.id}`)} 
                        className="bg-white p-4 rounded shadow cursor-pointer hover:bg-gray-50 flex justify-between">
                        <div>
                            <strong>{v.name}</strong> (ID: {v.id})
                        </div>
                        <div className="text-sm text-gray-500">
                            HP: {v.health} | $: {v.money}
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}