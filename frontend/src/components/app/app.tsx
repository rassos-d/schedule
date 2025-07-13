import './app.scss'
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { HelmetProvider } from 'react-helmet-async';
import { ToastContainer } from 'react-toastify';
import Main from '../../pages/main/main';
import Shedule from '../../pages/shedule/schedule';

function App() {

  return (
    <BrowserRouter>
      <HelmetProvider>
        <ToastContainer/>
        <Routes>
          <Route path='/' index element={<Main />} />
          <Route path='/:id' element={<Shedule/>}/>
        </Routes>
      </HelmetProvider>
    </BrowserRouter>
  )
}

export default App
