import { useEffect, useState } from 'react';
import { api } from './api';
import { useNavigate } from 'react-router-dom';

export default function ValeraList() {
    const [valeras, setValeras] = useState([]);
    const [search, setSearch] = useState('');
    const [showForm, setShowForm] = useState(false);
    const [form, setForm] = useState({ name: '', health: 100, mana: 0 });
    const navigate = useNavigate();

    const loadValeras = () => {
        api.get('/valera').then(res => setValeras(res.data));
    };

    useEffect(() => { loadValeras(); }, []);

    const handleCreate = async (e) => {
        e.preventDefault();
        await api.post('/valera', form);
        setShowForm(false);
        loadValeras();
    };

    const filtered = valeras.filter(v => v.name.toLowerCase().includes(search.toLowerCase()));

    return (
        <div className="max-w-2xl mx-auto">
            <h1 className="text-3xl font-bold mb-4">Список Валер</h1>
            
            <div className="flex gap-4 mb-4">
                <input 
                    type="text" placeholder="Поиск по имени..." 
                    className="border p-2 rounded w-full"
                    value={search} onChange={e => setSearch(e.target.value)}
                />
                <button onClick={() => setShowForm(!showForm)} className="bg-blue-500 text-white px-4 py-2 rounded">
                    Создать Валеру
                </button>
            </div>

            {showForm && (
                <form onSubmit={handleCreate} className="bg-white p-4 mb-4 rounded shadow flex flex-col gap-2">
                    <input type="text" placeholder="Имя" required className="border p-2"
                        onChange={e => setForm({...form, name: e.target.value})} />
                    <button type="submit" className="bg-green-500 text-white p-2 rounded">Сохранить</button>
                </form>
            )}

            <ul className="flex flex-col gap-2">
                {filtered.map(v => (
                    <li key={v.id} onClick={() => navigate(`/valera/${v.id}`)} 
                        className="bg-white p-4 rounded shadow cursor-pointer hover:bg-gray-50">
                        <strong>{v.name}</strong> (ID: {v.id})
                    </li>
                ))}
            </ul>
        </div>
    );
}