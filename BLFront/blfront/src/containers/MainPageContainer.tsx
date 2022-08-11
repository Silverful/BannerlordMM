import { useEffect, useState } from "react";
import { MainStatsItem } from "../typings/schema";
import { fetchGet } from "../util/apiFetch";


interface Props {

}

const getMainStats = () => {
    return fetchGet<MainStatsItem[]>('/api/stats');
}

const MainPageContainer : React.FC<Props> = (props: Props) => {
    const [stats, setStats] = useState<MainStatsItem[]>([]);
  
    useEffect(() => {
        getMainStats()
            .then(data => setStats(data))
    }, []);

    return (
        <MainStatsPage
            stats={stats}
        />)
}

export default MainPageContainer;