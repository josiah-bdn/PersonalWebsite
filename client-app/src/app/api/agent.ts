import axios  from   'axios';
import { RegisterDto } from '../models/dto/authentication/registerDto';

const sleep = (delay: number) => {
    return new Promise((resolve)=> {
        setTimeout(resolve, delay)
    })
}

axios.defaults.baseURL = 'https://localhost:7236/api';

axios.interceptors.response.use(async response => {
    try {
        await sleep(500);
        return response;
    } catch (error) {
        console.log(error);
        return await Promise.reject(error);
    }
})

// const responseBody = <T> (response: AxiosResponse<T>) => response.data; 

//  const requests = {
//     get: <T> (url: string) => axios.get<T>(url).then(responseBody),
//     post: <T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
//     put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
//     del: <T> (url: string) => axios.delete<T>(url).then(responseBody)
//  }

 const Authentication = {

    create: (RegisterDto: RegisterDto) => axios.post<void>('/Authentication/RegisterUser', RegisterDto),
 }

 const agent = {
    Authentication
 }

 export default agent; 