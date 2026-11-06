import { useEffect, useState } from 'react';
import { api } from './api';
import { useParams, useNavigate } from 'react-router-dom';

export default function ValeraStats() {
    const { id } = useParams();
    const [valera, setValera] = useState(null);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false); // Защита от двойных кликов
    const navigate = useNavigate();

    const loadValera = () => {
        api.get(`/valera/${id}`)
           .then(res => setValera(res.data))
           .catch((err) => {
               if (err.response?.status === 403) {
                   alert('Доступ запрещен! Это не ваша Валера.');
                   navigate('/');
               } else {
                   alert('Валера не найден');
                   navigate('/');
               }
           });
    };

    useEffect(() => { 
        loadValera(); 
    }, [id]);

    const doAction = async (action) => {
        if (isLoading) return; // Если уже идет запрос, игнорируем клик
        setIsLoading(true);
        setError('');
        try {
            await api.post(`/valera/${id}/${action}`);
            loadValera(); // Обновляем данные после действия
        } catch (err) {
            setError(err.response?.data?.error || 'Ошибка выполнения действия');
        } finally {
            setIsLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!window.confirm('Вы уверены, что хотите удалить этого Валеру?')) return;
        setIsLoading(true);
        try {
            await api.delete(`/valera/${id}`);
            navigate('/');
        } catch (err) {
            alert('Не удалось удалить. Возможно, нет прав.');
            setIsLoading(false);
        }
    };

    if (!valera) return <div className="text-center mt-10">Загрузка...</div>;

    // Условия для блокировки кнопок (по правилам игры)
    const isWorkDisabled = isLoading || valera.fatigue >= 10 || valera.mana >= 50;
    const isBarDisabled = isLoading || valera.money < 100;
    const isMarginalsDisabled = isLoading || valera.money < 150;

    const ProgressBar = ({ label, value, max, color }) => (
        <div className="mb-2">
            <div className="flex justify-between text-sm font-medium">
                <span>{label}</span>
                <span>{value}</span>
            </div>
            <div className="w-full bg-gray-300 rounded h-4 overflow-hidden">
                <div className={`h-4 rounded ${color} transition-all duration-300`} 
                     style={{ width: `${Math.min(100, Math.max(0, (value / max) * 100))}%` }}></div>
            </div>
        </div>
    );

    return (
        <div className="max-w-2xl mx-auto bg-white p-6 rounded shadow mt-10">
            <div className="flex justify-between items-center mb-4">
                <button onClick={() => navigate('/')} className="text-blue-500 hover:underline">
                    ← Назад к списку
                </button>
                <button onClick={handleDelete} disabled={isLoading} 
                        className="text-red-500 hover:text-red-700 text-sm font-bold disabled:opacity-50">
                    Удалить Валеру
                </button>
            </div>

            <h1 className="text-3xl font-bold mb-6 text-center">{valera.name} <span className="text-sm text-gray-400">(ID: {valera.id})</span></h1>

            {error && <div className="bg-red-100 border border-red-400 text-red-700 p-3 mb-4 rounded">{error}</div>}

            <div className="mb-8 p-4 bg-gray-50 rounded border">
                <ProgressBar label="Здоровье" value={valera.health} max={100} color="bg-green-500" />
                <ProgressBar label="Алкоголь (Мана)" value={valera.mana} max={100} color="bg-purple-500" />
                <ProgressBar label="Жизнерадостность" value={valera.cheerfulness + 10} max={20} color="bg-yellow-400" /> 
                <ProgressBar label="Усталость" value={valera.fatigue} max={100} color="bg-red-500" />
                <div className="text-xl font-bold mt-4 text-right">Деньги: <span className="text-green-600">${valera.money}</span></div>
            </div>

            <h2 className="text-xl font-bold mb-3 border-b pb-2">Действия</h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                <button onClick={() => doAction('work')} disabled={isWorkDisabled} 
                        className="bg-blue-600 hover:bg-blue-700 text-white p-2 rounded disabled:bg-gray-400 disabled:cursor-not-allowed transition">
                    Пойти на работу
                </button>
                
                <button onClick={() => doAction('contemplate')} disabled={isLoading} 
                        className="bg-green-600 hover:bg-green-700 text-white p-2 rounded disabled:opacity-50 transition">
                    Созерцать природу
                </button>
                
                <button onClick={() => doAction('drink-wine')} disabled={isLoading} 
                        className="bg-purple-600 hover:bg-purple-700 text-white p-2 rounded disabled:opacity-50 transition">
                    Пить вино и смотреть сериал
                </button>
                
                <button onClick={() => doAction('bar')} disabled={isBarDisabled} 
                        className="bg-pink-600 hover:bg-pink-700 text-white p-2 rounded disabled:bg-gray-400 disabled:cursor-not-allowed transition">
                    Сходить в бар (-$100)
                </button>
                
                <button onClick={() => doAction('drink-marginals')} disabled={isMarginalsDisabled} 
                        className="bg-gray-800 hover:bg-gray-900 text-white p-2 rounded disabled:bg-gray-400 disabled:cursor-not-allowed transition">
                    Выпить с маргиналами (-$150)
                </button>
                
                <button onClick={() => doAction('sing')} disabled={isLoading} 
                        className="bg-yellow-500 hover:bg-yellow-600 text-white p-2 rounded disabled:opacity-50 transition">
                    Петь в метро
                </button>
                
                <button onClick={() => doAction('sleep')} disabled={isLoading} 
                        className="bg-indigo-600 hover:bg-indigo-700 text-white p-2 rounded sm:col-span-2 disabled:opacity-50 transition">
                    Спать
                </button>
            </div>
            
            {isLoading && <p className="text-center mt-4 text-gray-500 animate-pulse">Выполняется запрос...</p>}
        </div>
    );
}