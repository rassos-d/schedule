import axios from 'axios';
import { getTokenFromCookie } from '../token';
import { toast } from 'react-toastify';


const BACKEND_URL = import.meta.env.VITE_BACKEND_URL

const notify = (message: string) => toast(message);

export const PagesURl = {
  AUDIENCE: 'audiences',
  DIRECTION: 'directions',
  EVENT: 'events',
  LESSON: 'lessons',
  SHEDULE: 'schedules',
  SQUARD: 'squards',
  SUBJECT: 'subjects',
  TEACHER: 'teachers',
  THEME: 'themes'
}

const REQUEST_TIMEOUT = 10000;


const instance = axios.create({
  baseURL: BACKEND_URL,
  timeout: REQUEST_TIMEOUT
});


instance.interceptors.response.use(
  response => response,
  async error => {

    if (error.message === `timeout of ${REQUEST_TIMEOUT}ms exceeded`) {
      console.log(error);
    } else {
      switch (error.response?.status) {
        case 404:
          console.log(error);
          break;
        case 403:
          break;
        case 400:
          if (error.response?.data?.detail) {
            console.log(error.response.data.detail)
            notify(error.response.data.detail);
          }
          break;
        case 405:
          
          break;
        default:
          console.log(error);
          if (error.code === 'ECONNABORTED') {
            window.location.href = 'error';
          }
      }
    }

    return Promise.reject(error);
  }
);

instance.interceptors.request.use(
  config => {
    config.timeout = REQUEST_TIMEOUT;
    config.headers.Authorization = 'Bearer ' + getTokenFromCookie('access');
    if (config.url){
      {
        config.params = {
          ...config.params,
        }
      }
    }
    //console.log(config)
    return config;
  },
/*   error => Promise.reject(error) */
);

export default instance;