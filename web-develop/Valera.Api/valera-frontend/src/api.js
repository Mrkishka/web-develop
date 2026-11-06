import axios from 'axios';

export const api = axios.create({
    baseURL: 'http://localhost:5291/api', // Твой порт
});

// ИНТЕРСЕПТОР: Автоматически добавляет токен в заголовки
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);