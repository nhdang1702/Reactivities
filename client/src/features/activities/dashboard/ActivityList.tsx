import ActivityCard from "./ActivityCard";
import { Box } from "@mui/material";

type Props = {
    activities: Activity[]; 
    selectActivity: (id: string) => void;
    deleteActivity: (id: string) => void;
}
export default function ActivityList({activities, selectActivity, deleteActivity} : Props) {
  return (
    <Box>
        {activities.map(activity => (<ActivityCard key={activity.id} activity={activity} selectActivity={selectActivity} deleteActivity={deleteActivity} />))}  
    </Box>
  )
}
