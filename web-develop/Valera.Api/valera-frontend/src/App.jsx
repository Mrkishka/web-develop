import { BrowserRouter, Routes, Route } from 'react-router-dom';
import ValeraList from './ValeraList';
import ValeraStats from './ValeraStats';
import './App.css'; // Оставь, если используешь, или удали, если стилизуешь только через Tailwind

function App() {
  return (
    <BrowserRouter>
      {/* Обертка для фона и отступов (использует классы Tailwind CSS) */}
      <div className="min-h-screen bg-gray-100 p-8">
        <Routes>
          {/* Главная страница — список всех Валер */}
          <Route path="/" element={<ValeraList />} />
          
          {/* Страница статистики конкретного Валеры по его ID */}
          <Route path="/valera/:id" element={<ValeraStats />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;