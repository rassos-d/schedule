import { Helmet } from 'react-helmet-async'
import styles from './main.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { useEffect, useState } from 'react'
import { SmallShedule } from '../../types/shedule'
import { Icon } from '../../components/icon'
import { Button } from '../../components/button/button'
import PopupContainer from '../../components/popupContainer/popupContainer'
import { Input } from '../../components/input/Input'
import { useNavigate } from 'react-router-dom'

export default function Main () {

    const [shedules, setShedules] = useState<SmallShedule[]>()
    const [newSchedule, setNewSchedule] = useState<string>()
    const navigate = useNavigate()


    const handleGetShedules = async () => {
        const {data} = await axios.get<SmallShedule[]>(PagesURl.SHEDULE + '/find')
        console.log(data)
        setShedules(data)
    }
    const handleDeleteSchedule = async (id: string) => {
        await axios.delete(PagesURl.SHEDULE + `/${id}`)
        handleGetShedules()
    }
    const handleCreateShedule = async (name: string) => {
        const {data} = await axios.post<{data: string}>(PagesURl.SHEDULE, {
            name: name
        })
        navigate(`/${data.data}`)
    }

    useEffect(()=>{
        handleGetShedules()
    },[])

    if (!shedules) {
        return <></>
    }

    return (
        <>
            <Helmet>
                <title>Главная</title>
            </Helmet>
            <div className={styles.container}>
                <h1 className={styles.container__title}>Расписание кафедры СП</h1>
                <h3 className={styles.container__subtitle}>Сохранённые расписания</h3>
                <div className={styles.container__shedules}>
                    {shedules.map((shedule)=>(
                        <div onClick={()=>navigate(`/${shedule.id}`)} className={styles.container__shedule} key={shedule.id}>
                            <p>{shedule.name}</p>
                            <div onClick={(e)=>{e.stopPropagation();handleDeleteSchedule(shedule.id)}}><Icon glyph='close' glyphColor='black'/></div>
                        </div>
                    ))}
                </div>
                <div onClick={()=>setNewSchedule('')} className={styles.container__button}><Button>Создать новое</Button></div>
            </div>
            {newSchedule !== undefined && 
                <PopupContainer>
                    <div className={styles.popup}>
                        <h2>Создание расписания</h2>
                        <Input value={newSchedule} onChange={setNewSchedule} placeholder='Введите название'/>
                        <Button onClick={()=>handleCreateShedule(newSchedule)}>Создать расписание</Button>
                    </div>
                </PopupContainer>
            }
        </>
    )
}