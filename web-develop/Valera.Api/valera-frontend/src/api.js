import axios from 'axios';

export const api = axios.create({
    baseURL: 'http://localhost:5291/api', // Укажи свой порт из dotnet run!
});