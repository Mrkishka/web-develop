import { useEffect, useState } from 'react';
import { api } from './api';
import { useParams, useNavigate } from 'react-router-dom';

export default function ValeraStats() {
    const { id } = useParams();
    const [valera, setValera] = useState(null);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const loadValera = () => {
        api.get(`/valera/${id}`).then(res => setValera(res.data)).catch(() => navigate('/'));
    };

    useEffect(() => { loadValera(); }, [id]);

    const doAction = async (action) => {
        setError('');
        try {
            await api.post(`/valera/${id}/${action}`);
            loadValera(); // Обновляем данные после действия
        } catch (err) {
            setError(err.response?.data?.error || 'Ошибка');
        }
    };

    if (!valera) return <div>Загрузка...</div>;

    const isWorkDisabled = valera.fatigue >= 10 || valera.mana >= 50;

    const ProgressBar = ({ label, value, max, color }) => (
        <div className="mb-2">
            <div className="flex justify-between"><span>{label}</span><span>{value}</span></div>
            <div className="w-full bg-gray-300 rounded h-4">
                <div className={`h-4 rounded ${color}`} style={{ width: `${(value / max) * 100}%` }}></div>
            </div>
        </div>
    );

    return (
        <div className="max-w-2xl mx-auto bg-white p-6 rounded shadow">
            <button onClick={() => navigate('/')} className="text-blue-500 mb-4">← Назад</button>
            <h1 className="text-2xl font-bold mb-4">{valera.name}</h1>

            {error && <div className="bg-red-100 text-red-700 p-2 mb-4 rounded">{error}</div>}

            <div className="mb-6">
                <ProgressBar label="Здоровье" value={valera.health} max={100} color="bg-green-500" />
                <ProgressBar label="Алкоголь (Мана)" value={valera.mana} max={100} color="bg-purple-500" />
                <ProgressBar label="Жизнерадостность" value={valera.cheerfulness + 10} max={20} color="bg-yellow-500" /> {/* +10 чтобы убрать минус */}
                <ProgressBar label="Усталость" value={valera.fatigue} max={100} color="bg-red-500" />
                <div className="text-xl font-bold mt-2">Деньги: ${valera.money}</div>
            </div>

            <h2 className="text-xl mb-2">Действия</h2>
            <div className="grid grid-cols-2 gap-2">
                <button onClick={() => doAction('work')} disabled={isWorkDisabled} className="bg-blue-500 text-white p-2 rounded disabled:opacity-50">Пойти на работу</button>
                <button onClick={() => doAction('contemplate')} className="bg-green-500 text-white p-2 rounded">Созерцать природу</button>
                <button onClick={() => doAction('drink-wine')} className="bg-purple-500 text-white p-2 rounded">Пить вино и смотреть сериал</button>
                <button onClick={() => doAction('bar')} className="bg-pink-500 text-white p-2 rounded">Сходить в бар</button>
                <button onClick={() => doAction('drink-marginals')} className="bg-gray-700 text-white p-2 rounded">Выпить с маргиналами</button>
                <button onClick={() => doAction('sing')} className="bg-yellow-500 text-white p-2 rounded">Петь в метро</button>
                <button onClick={() => doAction('sleep')} className="bg-indigo-500 text-white p-2 rounded col-span-2">Спать</button>
            </div>
        </div>
    );
}