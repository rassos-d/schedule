import { Helmet } from 'react-helmet-async'
import styles from './main.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { useEffect, useState } from 'react'
import { SmallShedule } from '../../types/shedule'

export default function Main () {

    const [shedules, setShedules] = useState<SmallShedule[]>()

    const handleGetShedules = async () => {
        const {data} = await axios.get<SmallShedule[]>(PagesURl.SHEDULE + '/find')
        console.log(data)
        setShedules(data)
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
                        <div key={shedule.id}>
                            <p>{shedule.name}</p>
                        </div>
                    ))}
                </div>
            </div>
        </>
    )
}